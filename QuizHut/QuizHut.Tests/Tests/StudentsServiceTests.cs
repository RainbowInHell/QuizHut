namespace QuizHut.Tests.Tests
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class StudentsServiceTests : SetUpTests
    {
        private IStudentsService Service => ServiceProvider.GetRequiredService<IStudentsService>();

        [Fact]
        public async Task GetStudentsAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            var teacherId = await CreateUserAsync("teacher@teacher.com", "Teacher", "Teacher", "Teacher");
            await CreateUserAsync("student@student.com", "Student", "Student");

            var model = new StudentViewModel()
            {
                Id = teacherId,
                FullName = "Teacher Teacher",
                Email = "teacher@teacher.com",
            };

            //Act
            var resultModelCollection = await Service.GetAllStudentsAsync<StudentViewModel>();

            //Assert
            Assert.Equal(model.Id, resultModelCollection.First().Id);
            Assert.Equal(model.FullName, resultModelCollection.First().FullName);
            Assert.Equal(model.Email, resultModelCollection.First().Email);
        }

        [Fact]
        public async Task GetAllStudentsByGroupIdAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            await CreateUserAsync("teacher@teacher.com", "Teacher", "Teachers");
            var studentId = await CreateUserAsync("student@student.com", "Student", "Student");
            var groupId = await AssignStudentToGroupAsync(studentId);

            var model = new StudentViewModel()
            {
                Id = studentId,
                FullName = "Student Student",
                Email = "student@student.com",
                IsAssigned = false,
            };

            //Act
            var resultModelCollection = await Service.GetAllStudentsByGroupIdAsync<StudentViewModel>(groupId);

            //Assert
            Assert.Equal(1, resultModelCollection.Count());
            Assert.Equal(model.Id, resultModelCollection.First().Id);
            Assert.Equal(model.FullName, resultModelCollection.First().FullName);
            Assert.Equal(model.Email, resultModelCollection.First().Email);
        }

        [Fact]
        public async Task AddStudentAsyncShould_ReturnTrueIfStudentIsSuccessfullyAdded()
        {
            //Arrange
            var teacherId = await CreateUserAsync("teacher@teacher.com", "Teacher", "Teachers");
            var studentId = await CreateUserAsync("student@student.com", "Student", "Student");

            //Act
            var result = await Service.AddStudentAsync("student@student.com", teacherId);

            //Assert
            var teacher = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == teacherId);
            var student = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == studentId);

            Assert.True(result);
            Assert.Equal(1, teacher.Students.Count);
            Assert.True(teacher.Students.Contains(student));
            Assert.Equal(teacherId, student.TeacherId);
        }

        [Fact]
        public async Task AddStudentAsync_ShouldReturnFalseIfStudentIsNotFound()
        {
            //Arrange
            var teacherId = await CreateUserAsync("teacher@teacher.com", "Teacher", "Teachers");

            //Act
            var result = await Service.AddStudentAsync("student@student.com", teacherId);

            //Assert
            var teacher = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == teacherId);

            Assert.False(result);
            Assert.Equal(0, teacher.Students.Count);
        }

        [Fact]
        public async Task DeleteStudentFromTeacherListAsync_ShouldRemoveStudentSuccessfully()
        {
            //Arrange
            var teacherId = await CreateUserAsync("teacher@teacher.com", "Teacher", "Teachers");
            var studentId = await CreateUserAsync("student@student.com", "Student", "Student");

            await AddStudentAsync(studentId, teacherId);
            
            //Act
            await Service.DeleteStudentFromTeacherListAsync(studentId, teacherId);

            //Assert
            var teacher = await GetUserAsync(teacherId);
            var student = await GetUserAsync(studentId);

            Assert.Equal(0, teacher.Students.Count);
            Assert.False(teacher.Students.Contains(student));
            Assert.Null(student.TeacherId);
        }

        private async Task<string> CreateUserAsync(string email, string firstName, string lastName, string roleName = null)
        {
            var user = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
            };

            if (roleName != null)
            {
                var role = new IdentityRole()
                {
                    Name = roleName,
                };

                await DbContext.Roles.AddAsync(role);
                var userRole = new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                };

                await DbContext.UserRoles.AddAsync(userRole);
                await DbContext.SaveChangesAsync();
                DbContext.Entry(role).State = EntityState.Detached;
                DbContext.Entry(userRole).State = EntityState.Detached;
            }

            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(user).State = EntityState.Detached;
            return user.Id;
        }

        private async Task AddStudentAsync(string studentId, string teacherId)
        {
            var teacher = await GetUserAsync(teacherId);
            var student = await GetUserAsync(studentId);
            
            teacher.Students.Add(student);
            
            student.TeacherId = teacherId;
            DbContext.Users.Update(student);
            DbContext.Users.Update(teacher);

            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(student).State = EntityState.Detached;
            DbContext.Entry(teacher).State = EntityState.Detached;
        }

        private async Task<ApplicationUser> GetUserAsync(string id)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            
            DbContext.Entry<ApplicationUser>(user).State = EntityState.Detached;
            
            return user;
        }

        private async Task<string> AssignStudentToGroupAsync(string studentId, string groupId = null)
        {
            Group group;
            if (groupId != null)
            {
                group = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            }
            else
            {
                group = await CreateGroupAsync();
            }

            var studentGroup = new StudentGroup() { StudentId = studentId, GroupId = group.Id };
            await DbContext.StudentsGroups.AddAsync(studentGroup);
            await DbContext.SaveChangesAsync();

            DbContext.Entry(studentGroup).State = EntityState.Detached;
            return group.Id;
        }

        private async Task<Group> CreateGroupAsync()
        {
            var group = new Group() { Name = "group", CreatorId = "asd" };

            await DbContext.Groups.AddAsync(group);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(group).State = EntityState.Detached;
            
            return group;
        }
    }
}
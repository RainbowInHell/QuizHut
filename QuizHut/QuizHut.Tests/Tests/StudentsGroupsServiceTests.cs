namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class StudentsGroupsServiceTests : SetUpTests
    {
        private IStudentsGroupsService Service => ServiceProvider.GetRequiredService<IStudentsGroupsService>();

        [Fact]
        public async Task CreateStudentGroupAsync_ShouldCreateNewStudentGroupIfDoesntExist()
        {
            //Arrange
            var groupId = await CreateGroupAsync();
            var studentId = await CreateStudentAsync();
            
            //Act
            await Service.CreateStudentGroupAsync(groupId, studentId);

            //Assert
            Assert.NotEmpty(DbContext.StudentsGroups);
        }

        [Fact]
        public async Task CreateStudentGroupAsync_ShouldNotCreateNewStudentGroupIfAlreadyExist()
        {
            //Arrange
            var groupId = await CreateGroupAsync();
            var studentId = await CreateStudentAsync();

            //Act
            await Service.CreateStudentGroupAsync(groupId, studentId);
            await Service.CreateStudentGroupAsync(groupId, studentId);

            //Assert
            var studentsGroupsCount = DbContext.StudentsGroups.ToArray().Count();

            Assert.Equal(1, studentsGroupsCount);
        }

        [Fact]
        public async Task DeleteStudentGroupAsync_ShouldDeleteCorrectly()
        {
            //Arrange
            var studentId = await CreateStudentAsync();
            var groupId = await CreateGroupAsync();
            await CreateStudentGroupAsync(studentId, groupId);

            //Act
            await Service.DeleteStudentGroupAsync(groupId, studentId);

            //Assert
            Assert.Empty(DbContext.StudentsGroups);
        }

        private async Task<string> CreateStudentAsync()
        {
            var student = new ApplicationUser()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                Email = "email@email.com",
                UserName = "email@email.com",
            };

            await DbContext.Users.AddAsync(student);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(student).State = EntityState.Detached;

            return student.Id;
        }

        private async Task<string> CreateGroupAsync()
        {
            var creatorId = Guid.NewGuid().ToString();
            var group = new Group() { Name = "Group", CreatorId = creatorId };

            await DbContext.Groups.AddAsync(group);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(group).State = EntityState.Detached;
            
            return group.Id;
        }

        private async Task CreateStudentGroupAsync(string studentId, string groupId)
        {
            var studentGroup = new StudentGroup()
            {
                StudentId = studentId,
                GroupId = groupId,
            };

            await DbContext.StudentsGroups.AddAsync(studentGroup);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(studentGroup).State = EntityState.Detached;
        }
    }
}

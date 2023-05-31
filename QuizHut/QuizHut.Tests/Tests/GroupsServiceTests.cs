namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class GroupsServiceTests : SetUpTests
    {
        private IGroupsService Service => ServiceProvider.GetRequiredService<IGroupsService>();

        [Fact]
        public async Task GetAllGroupsAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            var firstGroupId = await CreateGroupAsync(null, "First Group");
            var secondGroupId = await CreateGroupAsync(null, "Second Group");

            var firstModel = new GroupListViewModel()
            {
                Id = firstGroupId,
                Name = "First Group",
                StudentsCount = 0,
                EventsCount = 0,
            };

            var secondModel = new GroupListViewModel()
            {
                Id = secondGroupId,
                Name = "Second Group",
                StudentsCount = 0,
                EventsCount = 0,
            };

            //Act
            var resultModelCollection = await Service.GetAllGroupsAsync<GroupListViewModel>();

            //Assert
            Assert.Equal(2, resultModelCollection.Count());
            Assert.IsAssignableFrom<IList<GroupListViewModel>>(resultModelCollection);

            Assert.Equal(firstModel.Id, resultModelCollection.Last().Id);
            Assert.Equal(firstModel.Name, resultModelCollection.Last().Name);
            Assert.Equal(firstModel.StudentsCount, resultModelCollection.Last().StudentsCount);
            Assert.Equal(firstModel.EventsCount, resultModelCollection.Last().EventsCount);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.StudentsCount, resultModelCollection.First().StudentsCount);
            Assert.Equal(secondModel.EventsCount, resultModelCollection.First().EventsCount);
        }

        [Fact]
        public async Task GetAllGroupsAsync_ShouldReturnCorrectModelCollectionWhenCreatorIdIsPassed()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            await CreateGroupAsync(null, "First Group");
            var secondGroupId = await CreateGroupAsync(creatorId, "Second Group");

            var secondModel = new GroupListViewModel()
            {
                Id = secondGroupId,
                Name = "Second Group",
                StudentsCount = 0,
                EventsCount = 0,
            };

            //Act
            var resultModelCollection = await Service.GetAllGroupsAsync<GroupListViewModel>(creatorId);

            //Assert
            Assert.Single(resultModelCollection);
            Assert.IsAssignableFrom<IList<GroupListViewModel>>(resultModelCollection);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.StudentsCount, resultModelCollection.First().StudentsCount);
            Assert.Equal(secondModel.EventsCount, resultModelCollection.First().EventsCount);
        }

        [Fact]
        public async Task GetAllGroupsByEventIdAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            var eventId = Guid.NewGuid().ToString();
            var creatorId = Guid.NewGuid().ToString();

            var firstGroupId = await CreateGroupAsync(creatorId, "First Group");
            var secondGroupId = await CreateGroupAsync(creatorId, "Second Group");

            await AssignEventToGroupAsync(eventId, firstGroupId);
            await AssignEventToGroupAsync(eventId, secondGroupId);

            var firstModel = new GroupAssignViewModel()
            {
                Id = firstGroupId,
                Name = "First Group",
                CreatorId = creatorId,
                IsAssigned = false,
            };

            var secondModel = new GroupAssignViewModel()
            {
                Id = secondGroupId,
                Name = "Second Group",
                CreatorId = creatorId,
                IsAssigned = false,
            };

            //Act
            var resultModelCollection = await Service.GetAllGroupsByEventIdAsync<GroupAssignViewModel>(eventId);

            //Assert
            Assert.Equal(firstModel.Id, resultModelCollection.First().Id);
            Assert.Equal(firstModel.Name, resultModelCollection.First().Name);
            Assert.Equal(firstModel.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
            Assert.Equal(secondModel.Id, resultModelCollection.Last().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.Last().Name);
            Assert.Equal(secondModel.CreatorId, resultModelCollection.Last().CreatorId);
            Assert.False(resultModelCollection.Last().IsAssigned);
            Assert.Equal(2, resultModelCollection.Count());
        }

        [Fact]
        public async Task AssignStudentsToGroupAsync_ShouldAssignStudentsCorrectly()
        {
            //Arrange
            List<string> studentsIds = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                var studentId = await CreateStudentAsync();
                studentsIds.Add(studentId);
            }

            var groupId = await CreateGroupAsync();
            
            //Act
            await Service.AssignStudentsToGroupAsync(groupId, studentsIds);

            //Assert
            var groupAfterAssigningStudents = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            var idsOfAssignedStudents = groupAfterAssigningStudents.StudentsGroups.Select(x => x.StudentId);

            foreach (var id in studentsIds)
            {
                Assert.Contains(id, idsOfAssignedStudents);
            }

            Assert.Equal(5, groupAfterAssigningStudents.StudentsGroups.Count);
        }

        [Fact]
        public async Task AssignEventsToGroupAsync_ShouldAssignEventsCorrectly()
        {
            //Arrange
            List<string> eventsIds = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                var eventId = await CreateEventAsync($"Event {i}", DateTime.UtcNow);
                eventsIds.Add(eventId);
            }

            var groupId = await CreateGroupAsync();
            
            //Act
            await Service.AssignEventsToGroupAsync(groupId, eventsIds);

            //Assert
            var groupAfterAssigningEvents = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            var idsOfAssignedEvents = groupAfterAssigningEvents.EventsGroups.Select(x => x.EventId);

            foreach (var id in eventsIds)
            {
                Assert.Contains(id, idsOfAssignedEvents);
            }

            Assert.Equal(5, groupAfterAssigningEvents.EventsGroups.Count);
        }

        [Fact]
        public async Task CreateGroupAsync_ShouldCreateCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            
            //Act
            await Service.CreateGroupAsync("Test Group", creatorId);

            //Assert
            var group = await DbContext.Groups.FirstOrDefaultAsync();
            
            Assert.NotNull(group);
            Assert.Equal("Test Group", group.Name);
            Assert.Equal(creatorId, group.CreatorId);
            Assert.Equal(1, DbContext.Groups.Count());
        }

        [Fact]
        public async Task UpdateGroupNameAsync_ShouldChangeTheNameOfTheGroupCorrectly()
        {
            //Arrange
            var groupId = await CreateGroupAsync(null, "group");
            
            //Act
            await Service.UpdateGroupNameAsync(groupId, "Test Group");

            //Assert
            var updatedGroup = await DbContext.Groups.FirstOrDefaultAsync();
            
            Assert.Equal("Test Group", updatedGroup.Name);
        }

        [Fact]
        public async Task DeleteGroupAsync_ShouldDeleteCorrectly()
        {
            //Arrange
            var groupId = await CreateGroupAsync();

            //Act
            await Service.DeleteGroupAsync(groupId);

            //Assert
            var group = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            
            Assert.Null(group);
            Assert.Equal(0, DbContext.Groups.Count());
        }

        [Fact]
        public async Task DeleteStudentFromGroupAsync_ShouldRemoveCurrentStudentFromGroup()
        {
            //Arrange
            var groupId = await CreateGroupAsync();
            List<string> studentsIds = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                var studentId = await CreateStudentAsync();
                studentsIds.Add(studentId);
                await AssignStudentToGroupAsync(studentId, groupId);
            }

            //Act
            await Service.DeleteStudentFromGroupAsync(groupId, studentsIds[0]);

            //Assert
            var studentsInGroupsAfterDeleteStudent = await DbContext.Groups
                .Where(x => x.Id == groupId).Select(x => x.StudentsGroups.Select(x => x.StudentId)).FirstOrDefaultAsync();

            Assert.DoesNotContain(studentsIds[0], studentsInGroupsAfterDeleteStudent);
            Assert.Equal(4, studentsInGroupsAfterDeleteStudent.Count());
        }

        [Fact]
        public async Task DeleteEventFromGroupAsync_ShouldRemoveCurrentEventFromGroup()
        {
            //Arrange
            var groupId = await CreateGroupAsync();
            List<string> eventsIds = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                var eventId = await CreateEventAsync($"Event {i}", DateTime.UtcNow);
                eventsIds.Add(eventId);
                await AssignEventToGroupAsync(eventId, groupId);
            }

            //Act
            await Service.DeleteEventFromGroupAsync(groupId, eventsIds[0]);

            //Assert
            var eventsInGroupsAfterDeleteEvent = await DbContext.Groups
                .Where(x => x.Id == groupId).Select(x => x.EventsGroups.Select(x => x.EventId)).FirstOrDefaultAsync();

            Assert.DoesNotContain(eventsIds[0], eventsInGroupsAfterDeleteEvent);
            Assert.Equal(4, eventsInGroupsAfterDeleteEvent.Count());
        }

        private async Task<string> CreateGroupAsync(string creatorId = null, string name = null)
        {
            var group = new Group()
            {
                Name = name ?? "group",
                CreatorId = creatorId ?? Guid.NewGuid().ToString(),
            };

            await DbContext.Groups.AddAsync(group);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(group).State = EntityState.Detached;
            
            return group.Id;
        }

        private async Task<string> CreateEventAsync(string name, DateTime activation)
        {
            var creatorId = Guid.NewGuid().ToString();

            var @event = new Event
            {
                Name = name,
                Status = Status.Pending,
                ActivationDateAndTime = activation,
                DurationOfActivity = TimeSpan.FromMinutes(30),
                CreatorId = creatorId,
            };

            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(@event).State = EntityState.Detached;

            return @event.Id;
        }

        private async Task AssignEventToGroupAsync(string eventId, string groupId)
        {
            var eventGroup = new EventGroup() { EventId = eventId, GroupId = groupId };
            
            await DbContext.EventsGroups.AddAsync(eventGroup);
            await DbContext.SaveChangesAsync();
           
            DbContext.Entry(eventGroup).State = EntityState.Detached;
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

        private async Task AssignStudentToGroupAsync(string studentId, string groupId)
        {
            var studentGroup = new StudentGroup() { StudentId = studentId, GroupId = groupId };
            
            await DbContext.StudentsGroups.AddAsync(studentGroup);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry<StudentGroup>(studentGroup).State = EntityState.Detached;
        }
    }
}
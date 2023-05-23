namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class EventsGroupsServiceTests : SetUpTests
    {
        private IEventsGroupsService Service => ServiceProvider.GetRequiredService<IEventsGroupsService>();

        [Fact]
        public async Task CreateEventGroupAsync_ShouldCreateNewEventGroup()
        {
            //Arrange
            var eventId = await CreateEventAsync();
            var groupId = await CreateGroupAsync();
            
            //Act
            await Service.CreateEventGroupAsync(eventId, groupId);

            //Assert
            var eventsGroupsCount = DbContext.EventsGroups.ToArray().Count();

            Assert.Equal(1, eventsGroupsCount);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCorrectly()
        {
            //Arrange
            var eventId = await CreateEventAsync();
            var groupId = await CreateGroupAsync();
            
            await CreateEventGroupAsync(groupId, eventId);

            //Act
            await Service.DeleteEventGroupAsync(eventId, groupId);

            //Assert
            var eventsGroupsCount = DbContext.EventsGroups.ToArray().Count();
            
            Assert.Equal(0, eventsGroupsCount);
        }

        private async Task<string> CreateEventAsync()
        {
            var creatorId = Guid.NewGuid().ToString();

            var @event = new Event
            {
                Name = "Name",
                Status = Status.Pending,
                ActivationDateAndTime = DateTime.UtcNow,
                DurationOfActivity = TimeSpan.FromMinutes(30),
                CreatorId = creatorId,
            };

            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(@event).State = EntityState.Detached;
            
            return @event.Id;
        }

        private async Task<string> CreateGroupAsync()
        {
            var creatorId = Guid.NewGuid().ToString();
            var group = new Group() { Name = "Group", CreatorId = creatorId };
            
            await DbContext.Groups.AddAsync(group);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry<Group>(group).State = EntityState.Detached;
         
            return group.Id;
        }

        private async Task CreateEventGroupAsync(string groupId, string eventId)
        {
            var eventGroup = new EventGroup()
            {
                EventId = eventId,
                GroupId = groupId,
            };

            await DbContext.EventsGroups.AddAsync(eventGroup);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry<EventGroup>(eventGroup).State = EntityState.Detached;
        }
    }
}
namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class EventsServiceTests : SetUpTests
    {
        private IEventsService Service => ServiceProvider.GetRequiredService<IEventsService>();

        [Fact]
        public async Task GetAllEvents_ShouldFilterCorrectltyByCreatorId()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var secondEventDate = DateTime.UtcNow;

            await CreateEventAsync("First Event", DateTime.UtcNow, Guid.NewGuid().ToString());
            var secondEventId = await CreateEventAsync("Second Event", secondEventDate, creatorId);

            var secondModel = new EventListViewModel()
            {
                Id = secondEventId,
                Name = "Second Event",
                ActivationDateAndTime = secondEventDate,
                DurationOfActivity = TimeSpan.FromMinutes(30),
                Status = Status.Pending,
            };

            //Act
            var resultModelCollection = await Service.GetAllEventsAsync<EventListViewModel>(creatorId);

            //Assert
            Assert.Single(resultModelCollection);
            Assert.IsAssignableFrom<IList<EventListViewModel>>(resultModelCollection);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.ActivationDateAndTime, resultModelCollection.First().ActivationDateAndTime);
            Assert.Equal(secondModel.DurationOfActivity, resultModelCollection.First().DurationOfActivity);
            Assert.Equal(secondModel.Status, resultModelCollection.First().Status);
        }

        [Fact]
        public async Task GetAllFiteredByStatusAndGroupAsync_ShouldFilterCorrectllyAndReturnCorrectModelCollectionWhenCreatorIdIsNull()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var secondEventDate = DateTime.UtcNow;

            var firstEventId = await CreateEventAsync("First Event", DateTime.UtcNow, creatorId);
            var secondEventId = await CreateEventAsync("Second Event", secondEventDate, creatorId);
            var groupId = await CreateGroupAsync();
            await CreateEventGroupAsync(firstEventId, groupId);

            var secondModel = new EventsAssignViewModel()
            {
                Id = secondEventId,
                CreatorId = creatorId,
                Name = "Second Event",
                IsAssigned = false,
            };

            //Act
            var resultModelCollection = await Service.GetAllEventsFilteredByStatusAndGroupAsync<EventsAssignViewModel>(Status.Active, groupId);

            //Assert
            Assert.Single(resultModelCollection);
            Assert.IsAssignableFrom<IList<EventsAssignViewModel>>(resultModelCollection);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
        }

        [Fact]
        public async Task GetAllFiteredByStatusAndGroupAsync_ShouldFilterCorrectllyAndReturnCorrectModelCollectionWhenCreatorIdIsPassed()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var secondEventDate = DateTime.UtcNow;

            var firstEventId = await CreateEventAsync("First Event", DateTime.UtcNow, creatorId);
            var secondEventId = await CreateEventAsync("Second Event", secondEventDate, creatorId);
            await CreateEventAsync("Third Event", DateTime.UtcNow, Guid.NewGuid().ToString());

            var groupId = await CreateGroupAsync();
            await CreateEventGroupAsync(firstEventId, groupId);

            var secondModel = new EventsAssignViewModel()
            {
                Id = secondEventId,
                CreatorId = creatorId,
                Name = "Second Event",
                IsAssigned = false,
            };

            //Act
            var resultModelCollection = await Service.GetAllEventsFilteredByStatusAndGroupAsync<EventsAssignViewModel>(Status.Active, groupId, creatorId);

            //Assert
            Assert.Single(resultModelCollection);
            Assert.IsAssignableFrom<IList<EventsAssignViewModel>>(resultModelCollection);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
        }

        [Fact]
        public async Task GetAllEventsByGroupIdAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();

            await CreateEventAsync("First Event", DateTime.UtcNow, creatorId);
            var secondEventId = await CreateEventAsync("Second Event", DateTime.UtcNow, creatorId);
            await CreateEventAsync("Third Event", DateTime.UtcNow, Guid.NewGuid().ToString());

            var groupId = await CreateGroupAsync();
            await CreateEventGroupAsync(secondEventId, groupId);

            var secondModel = new EventsAssignViewModel()
            {
                Id = secondEventId,
                CreatorId = creatorId,
                Name = "Second Event",
                IsAssigned = false,
            };

            //Act
            var resultModelCollection = await Service.GetAllEventsByGroupIdAsync<EventsAssignViewModel>(groupId);

            //Assert
            Assert.Single(resultModelCollection);
            Assert.IsAssignableFrom<IList<EventsAssignViewModel>>(resultModelCollection);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
        }

        [Fact]
        public async Task AssigQuizToEventAsync_ShouldSetQuizToEventCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var quiz = await CreateQuizAsync();
            var @event = new Event
            {
                Name = "Event",
                Status = Status.Pending,
                ActivationDateAndTime = DateTime.UtcNow,
                DurationOfActivity = TimeSpan.FromMinutes(30),
                CreatorId = creatorId,
            };
            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(@event).State = EntityState.Detached;

            //Act
            await Service.AssignQuizzesToEventAsync(new List<string>() { quiz.Id }, @event.Id);

            //Assert
            var eventWithAssignedQuiz = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == @event.Id);

            Assert.NotEmpty(eventWithAssignedQuiz.Quizzes);
        }

        [Fact]
        public async Task AssignGroupsToEventAsync_ShouldAssignGroupsCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var eventId = await CreateEventAsync("First Event", DateTime.UtcNow, creatorId);
            var groupsIds = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                var groupId = await CreateGroupAsync();
                groupsIds.Add(groupId);
            }

            //Act
            await Service.AssignGroupsToEventAsync(groupsIds, eventId);

            //Assert
            var assignedGroupsIds = await DbContext.Events
                .Where(x => x.Id == eventId)
                .Select(x => x.EventsGroups.Select(x => x.GroupId))
                .FirstOrDefaultAsync();

            foreach (var id in groupsIds)
            {
                Assert.Contains(id, assignedGroupsIds);
            }
        }

        [Fact]
        public async Task CreateEventAsync_ShouldCreateEventCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var expectedEventName = "Test Event";
            var eventActivationDate = "01/04/2020";
            var activeFrom = "08:00";
            var activeTo = "10:00";

            var expectedEventActivationDate = new DateTime(2020, 4, 1, 5, 00, 00);
            var expectedEventDuration = new TimeSpan(2, 0, 0);

            //Act
            var eventId = await Service.CreateEventAsync(
                expectedEventName,
                eventActivationDate,
                activeFrom,
                activeTo,
                creatorId);

            //Assert
            var @event = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

            Assert.NotNull(@event);
            Assert.Equal(expectedEventName, @event.Name);
            Assert.Equal(expectedEventActivationDate, @event.ActivationDateAndTime);
            Assert.Equal(expectedEventDuration, @event.DurationOfActivity);
            Assert.Equal(Status.Pending, @event.Status);
            Assert.Equal(creatorId, @event.CreatorId);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateEventCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var eventId = await CreateEventAsync("Event", DateTime.UtcNow, creatorId);
            var expectedEventName = "Test Event";
            var eventActivationDate = "01/04/2020";
            var activeFrom = "08:00";
            var activeTo = "10:00";

            //Act
            await Service.UpdateEventAsync(eventId, expectedEventName, eventActivationDate, activeFrom, activeTo);

            //Assert
            var expectedEventActivationDate = new DateTime(2020, 4, 1, 5, 00, 00);
            var expectedEventDuration = new TimeSpan(2, 0, 0);

            var updatedEvent = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

            Assert.Equal(expectedEventName, updatedEvent.Name);
            Assert.Equal(expectedEventActivationDate, updatedEvent.ActivationDateAndTime);
            Assert.Equal(expectedEventDuration, updatedEvent.DurationOfActivity);
        }

        [Fact]
        public async Task DeleteQuizFromEventAsync_ShouldSetQuizzesToEmpty()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var quiz = await CreateQuizAsync();
            var eventId = await CreateEventAsync("Event", DateTime.UtcNow, creatorId, quiz.Id);
            
            //Act
            await Service.DeleteQuizFromEventAsync(eventId, quiz.Id);

            //Assert
            var @event = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

            Assert.NotNull(@event);
            Assert.Empty(@event.Quizzes);
        }

        [Fact]
        public async Task DeleteQuizFromEventAsync_IsActive_ShouldChangeEventStatusToPending()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var quiz = await CreateQuizAsync();
            var eventId = await CreateEventAsync("Event", DateTime.UtcNow, creatorId, quiz.Id);
            await ChangeEventStatus(eventId, Status.Active);

            //Act
            await Service.DeleteQuizFromEventAsync(eventId, quiz.Id);

            //Assert
            var @event = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

            Assert.Equal(Status.Pending, @event.Status);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var firstEventId = await CreateEventAsync("First Event", DateTime.UtcNow, creatorId);
           
            //Act
            await Service.DeleteEventAsync(firstEventId);

            //Assert
            var eventsCount = DbContext.Events.ToArray().Count();

            Assert.Equal(0, eventsCount);
        }

        [Fact]
        public void GetTimeErrorMessage_DateIsBeforeDateNow_ShouldReturnInvalidActivationDateMessage()
        {
            //Arrange
            var invalidActivationDateMessage = "Invalid Activation Date";

            //Act
            var resultMessage = Service.GetTimeErrorMessage("08:00", "22:00", "01/01/2000");
            
            //Assert
            Assert.Equal(invalidActivationDateMessage, resultMessage);
        }

        [Fact]
        public void GetTimeErrorMessage_BeforeOrEqualToStartingTime_ShouldReturnInvalidDurationOfActivityMessage()
        {
            //Arrange
            var invalidDurationOfActivityMessage = "Invalid Duration Of Activity";

            //Act
            var caseWhenAreEquelResultMessage = Service.GetTimeErrorMessage(
                DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1)).ToString(),
                DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1)).ToString(),
                DateTime.Now.Date.ToString("dd/MM/yyyy"));

            var caseWhenEndingTimeIsBeforeStartingResultMessage = Service.GetTimeErrorMessage(
                DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1)).ToString(),
                DateTime.Now.TimeOfDay.ToString(),
                DateTime.Now.Date.ToString("dd/MM/yyyy"));

            //Assert
            Assert.Equal(invalidDurationOfActivityMessage, caseWhenAreEquelResultMessage);
            Assert.Equal(invalidDurationOfActivityMessage, caseWhenEndingTimeIsBeforeStartingResultMessage);
        }

        [Fact]
        public void GetTimeErrorMessage_ShouldReturnNullIfAllTheChecksPassed()
        {
            //Act
            var resultMessageWhenStartTimeIsEquelToTimeNowMinutes = Service.GetTimeErrorMessage(
                DateTime.Now.TimeOfDay.ToString(),
                "23:59",
                DateTime.Now.Date.ToString("dd/MM/yyyy"));

            var resultMessageWhenStartTimeIsAfterTimeNowMinutes = Service.GetTimeErrorMessage(
               DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1)).ToString(),
               "23:59",
               DateTime.Now.Date.ToString("dd/MM/yyyy"));

            //Assert
            Assert.Null(resultMessageWhenStartTimeIsEquelToTimeNowMinutes);
            Assert.Null(resultMessageWhenStartTimeIsAfterTimeNowMinutes);
        }

        private async Task<string> CreateEventAsync(string name, DateTime activation, string creatorId, string quizId = null)
        {
            Quiz quiz;
            if (quizId != null)
            {
                quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            }
            else
            {
                quiz = await CreateQuizAsync();
            }

            var @event = new Event
            {
                Name = name,
                Status = Status.Pending,
                ActivationDateAndTime = activation,
                DurationOfActivity = TimeSpan.FromMinutes(30),
                CreatorId = creatorId
            };

            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry<Quiz>(quiz).State = EntityState.Detached;
            DbContext.Entry(@event).State = EntityState.Detached;
            
            return @event.Id;
        }

        private async Task ChangeEventStatus(string eventId, Status status)
        {
            var @event = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);
            @event.Status = status;
            
            DbContext.Update(@event);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(@event).State = EntityState.Detached;
        }

        private async Task<string> CreateGroupAsync()
        {
            var group = new Group()
            {
                Name = "New Group",
                CreatorId = Guid.NewGuid().ToString(),
            };

            await DbContext.Groups.AddAsync(group);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(group).State = EntityState.Detached;
            
            return group.Id;
        }

        private async Task<Quiz> CreateQuizAsync()
        {
            var quiz = new Quiz()
            {
                Name = "quiz",
                CreatorId = new Guid().ToString(),
                Description = "Description",
                Password = "Password"
            };

            await DbContext.Quizzes.AddAsync(quiz);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(quiz).State = EntityState.Detached;
            
            return quiz;
        }

        private async Task<EventGroup> CreateEventGroupAsync(string eventId, string groupId)
        {
            var eventGroup = new EventGroup() { EventId = eventId, GroupId = groupId };
            
            await DbContext.EventsGroups.AddAsync(eventGroup);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(eventGroup).State = EntityState.Detached;
            
            return eventGroup;
        }
    }
}

namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class ResultsServiceTests : SetUpTests
    {
        private IResultsService Service => ServiceProvider.GetRequiredService<IResultsService>();

        //[Theory]
        //[InlineData("EventName", null)]
        //[InlineData("QuizName", null)]
        //[InlineData("EventName", "event")]
        //[InlineData("EventName", "EvEnt")]
        //[InlineData("EventName", "n")]
        //public async Task GetResultsCountByStudentIdAsync_ShouldReturnCorrectCountWithSearchCriteriaAndSerchTextPassed(string searchCriteria, string searchText)
        //{
        //    var studentId = await CreateStudentAsync();
        //    var firstEventInfo = await CreateEventAsync("First Event", DateTime.UtcNow);
        //    var secondEventInfo = await CreateEventAsync("Second Event", DateTime.UtcNow);

        //    await CreateResultAsync(studentId, 2, 10, firstEventInfo[0]);
        //    await CreateResultAsync(studentId, 5, 10, secondEventInfo[0]);

        //    var count = await Service.GetResultsCountByStudentIdAsync(studentId, searchCriteria, searchText);

        //    Assert.Equal(2, count);
        //}

        [Fact]
        public async Task CreateResultAsync_ShouldCreateNewResult()
        {
            var studentId = Guid.NewGuid().ToString();
            var eventInfo = await CreateEventAsync("event", DateTime.UtcNow);
            var quizId = eventInfo[1];

            var newResultId = await Service.CreateResultAsync(studentId, quizId);
            var resultsCount = DbContext.Results.ToArray().Count();
            Assert.Equal(1, resultsCount);
            Assert.NotNull(await DbContext.Results.FirstOrDefaultAsync(x => x.Id == newResultId));
        }

        private async Task<string[]> CreateEventAsync(string name, DateTime activation)
        {
            var creatorId = Guid.NewGuid().ToString();
            var quiz = new Quiz()
            {
                Name = "quiz",
                CreatorId = creatorId,
                Description = name,
                Password = "asdasd"
            };

            var @event = new Event
            {
                Name = name,
                Status = Status.Pending,
                ActivationDateAndTime = activation,
                DurationOfActivity = TimeSpan.FromMinutes(30),
                CreatorId = creatorId
            };

            @event.Quizzes.Add(quiz);
            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();
            
            return new string[] { @event.Id, quiz.Id };
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

        //private async Task CreateResultAsync(string studentId, int points, int maxPoints, string eventId)
        //{
        //    var @event = await DbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

        //    var result = new Result()
        //    {
        //        Points = points,
        //        StudentId = studentId,
        //        MaxPoints = maxPoints,
        //        EventId = eventId,
        //        //EventName = @event.Name,
        //        //EventActivationDateAndTime = @event.ActivationDateAndTime,
        //        //QuizName = "asd"
        //    };

        //    await DbContext.Results.AddAsync(result);

        //    @event.Results.Add(result);
        //    DbContext.Update(@event);
        //    await DbContext.SaveChangesAsync();
            
        //    DbContext.Entry(result).State = EntityState.Detached;
        //}
    }
}
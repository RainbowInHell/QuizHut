namespace QuizHut.BLL.Services
{
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;
    
    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common.Enumerations;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;
    
    using TimeZoneConverter;

    public class EventsService : IEventsService
    {
        private readonly IDeletableEntityRepository<Event> repository;

        private readonly IQuizzesService quizService;

        private readonly IEventsGroupsService eventsGroupsService;

        private readonly IScheduledJobsService scheduledJobsService;

        private readonly IExpressionBuilder expressionBuilder;

        public EventsService(
            IDeletableEntityRepository<Event> repository,
            IQuizzesService quizService,
            IEventsGroupsService eventsGroupsService,
            IScheduledJobsService scheduledJobsService,
            IExpressionBuilder expressionBuilder)
        {
            this.repository = repository;
            this.quizService = quizService;
            this.eventsGroupsService = eventsGroupsService;
            this.scheduledJobsService = scheduledJobsService;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<IList<T>> GetAllEventsAsync<T>(
            string creatorId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var query = repository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<Event>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query
                   .OrderByDescending(x => x.CreatedOn)
                   .To<T>()
                   .ToListAsync();
        }

        public async Task<IList<T>> GetAllFilteredByStatusAndGroupAsync<T>(
            Status status, 
            string groupId, 
            string creatorId = null)
        {
            var query = repository
                .AllAsNoTracking()
                .Where(x => !x.EventsGroups
                .Any(x => x.GroupId == groupId));

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            return await query
              .Where(x => x.Status != status)
              .OrderByDescending(x => x.CreatedOn)
              .To<T>()
              .ToListAsync();
        }

        public async Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId)
        {
            return await repository
                .AllAsNoTracking()
                .Where(x => x.EventsGroups.Any(x => x.GroupId == groupId))
                .To<T>()
                .ToListAsync();
        }

        public async Task AssignGroupsToEventAsync(IList<string> groupIds, string eventId)
        {
            foreach (var groupId in groupIds)
            {
                await eventsGroupsService.CreateEventGroupAsync(eventId, groupId);
            }
        }

        public async Task AssignQuizToEventAsync(string eventId, string quizId)
        {
            var @event = await repository
                .All()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();

            @event.QuizId = quizId;
            @event.QuizName = await quizService.GetQuizNameByIdAsync(quizId);
            @event.Status = GetStatus(@event.ActivationDateAndTime, @event.DurationOfActivity, quizId);

            repository.Update(@event);
            await repository.SaveChangesAsync();

            if (@event.Status != Status.Ended)
            {
                await SheduleStatusChangeAsync(@event.ActivationDateAndTime, @event.DurationOfActivity, @event.Id, @event.Status);
            }

            await quizService.AssignQuizToEventAsync(eventId, quizId);
        }

        public async Task<string> CreateEventAsync(
            string name, 
            string activationDate, 
            string activeFrom, 
            string activeTo, 
            string creatorId)
        {
            var activationDateAndTime = GetActivationDateAndTimeUtc(activationDate, activeFrom);
            var durationOfActivity = GetDurationOfActivity(activationDate, activeFrom, activeTo);

            var @event = new Event
            {
                Name = name,
                Status = Status.Pending,
                ActivationDateAndTime = activationDateAndTime,
                DurationOfActivity = durationOfActivity,
                CreatorId = creatorId,
            };

            await repository.AddAsync(@event);
            await repository.SaveChangesAsync();

            return @event.Id;
        }

        public async Task UpdateAsync(string id, string name, string activationDate, string activeFrom, string activeTo)
        {
            var @event = await repository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var activationDateAndTime = GetActivationDateAndTimeUtc(activationDate, activeFrom);
            var durationOfActivity = GetDurationOfActivity(activationDate, activeFrom, activeTo);

            @event.Name = name;
            @event.ActivationDateAndTime = activationDateAndTime;
            @event.DurationOfActivity = durationOfActivity;
            @event.Status = GetStatus(activationDateAndTime, durationOfActivity, @event.QuizId);

            repository.Update(@event);
            await repository.SaveChangesAsync();

            if (@event.QuizId != null)
            {
                await SheduleStatusChangeAsync(activationDateAndTime, durationOfActivity, id, @event.Status);
            }
        }

        public async Task DeleteQuizFromEventAsync(string eventId, string quizId)
        {
            var @event = await repository
                .All()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();

            @event.QuizId = null;

            if (@event.Status == Status.Active)
            {
                @event.Status = Status.Pending;
                await scheduledJobsService.DeleteJobsAsync(@event.Id, true);
            }

            repository.Update(@event);

            await repository.SaveChangesAsync();
            
            await quizService.DeleteEventFromQuizAsync(eventId, quizId);
        }

        public async Task DeleteAsync(string eventId)
        {
            var @event = await repository
                .All()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();

            var quizId = @event.QuizId;
            if (quizId != null)
            {
                await quizService.DeleteEventFromQuizAsync(eventId, quizId);
            }

            repository.Delete(@event);
            await repository.SaveChangesAsync();
        }

        public string GetTimeErrorMessage(string activeFrom, string activeTo, string activationDate)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(DateTimeConverter.TimeZoneIana));
            var userLocalTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            var activationDateAndTimeUtc = GetActivationDateAndTimeUtc(activationDate, activeFrom);
            var activationDateAndTimeToUserLocalTime = TimeZoneInfo.ConvertTimeFromUtc(activationDateAndTimeUtc, zone);

            if (userLocalTimeNow.Date > activationDateAndTimeToUserLocalTime.Date)
            {
                return "Invalid Activation Date";
            }

            var timeToStart = TimeSpan.Parse(activeFrom);
            var timeNow = userLocalTimeNow.TimeOfDay;
            var startHours = timeToStart.Hours;
            var nowHours = timeNow.Hours;
            var startMins = timeToStart.Minutes;
            var nowMins = timeNow.Minutes;
            var invalidStartingTime = startHours < nowHours || (startHours == nowHours && startMins < nowMins);

            //if (userLocalTimeNow.Date == activationDateAndTimeToUserLocalTime.Date && invalidStartingTime)
            //{
            //    return "Invalid Starting Time";
            //}

            var duration = GetDurationOfActivity(activationDate, activeFrom, activeTo);
            if (duration.Hours <= 0 && duration.Minutes <= 0)
            {
                return "Invalid Duration Of Activity";
            }

            return null;
        }

        #region Help Methods

        private DateTime GetActivationDateAndTimeUtc(string activationDate, string activeFrom)
        {
            return DateTime.ParseExact(activationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                           .Add(TimeSpan.Parse(activeFrom))
                           .ToUniversalTime();
        }

        private TimeSpan GetDurationOfActivity(string activationDate, string activeFrom, string activeTo)
        {
            return DateTime.ParseExact(activationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                           .Add(TimeSpan.Parse(activeTo))
                           .ToUniversalTime() - GetActivationDateAndTimeUtc(activationDate, activeFrom);
        }

        private Status GetStatus(DateTime activationDateAndTime, TimeSpan durationOfActivity, string quizId)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(DateTimeConverter.TimeZoneIana));
            var userLocalTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            var activationDateAndTimeToUserLocalTime = TimeZoneInfo.ConvertTimeFromUtc(activationDateAndTime, zone);

            if (quizId == null
                || userLocalTimeNow.Date < activationDateAndTimeToUserLocalTime.Date
                || activationDateAndTimeToUserLocalTime.TimeOfDay > userLocalTimeNow.TimeOfDay)
            {
                return Status.Pending;
            }

            var startHours = activationDateAndTimeToUserLocalTime.TimeOfDay.Hours;
            var nowHours = userLocalTimeNow.TimeOfDay.Hours;
            var startMins = activationDateAndTimeToUserLocalTime.TimeOfDay.Minutes;
            var nowMins = userLocalTimeNow.TimeOfDay.Minutes;

            var endHours = activationDateAndTimeToUserLocalTime.Add(durationOfActivity).TimeOfDay.Hours;
            var endMinutes = activationDateAndTimeToUserLocalTime.Add(durationOfActivity).TimeOfDay.Minutes;

            if (startHours <= nowHours && startMins <= nowMins
                && (endHours > nowHours || (endHours == nowHours && endMinutes >= nowMins)))
            {
                return Status.Active;
            }

            return Status.Ended;
        }

        private async Task<string[]> GetStudentsNamesByEventIdAsync(string id)
        {
            return await repository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .SelectMany(x => x.EventsGroups.SelectMany(x => x.Group.StudentsGroups.Select(x => x.Student.UserName)))
                .ToArrayAsync();
        }

        private async Task SheduleStatusChangeAsync(
            DateTime activationDateAndTime,
            TimeSpan durationOfActivity,
            string eventId,
            Status eventStatus)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(DateTimeConverter.TimeZoneIana));
            var userLocalTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            var userTimeToUtc = userLocalTimeNow.ToUniversalTime();

            var activationDelay = activationDateAndTime - userTimeToUtc;
            var endingDelay = activationDateAndTime.Add(durationOfActivity) - userTimeToUtc;

            if (eventStatus == Status.Active)
            {
                await scheduledJobsService.DeleteJobsAsync(eventId, false);
                await scheduledJobsService.CreateEndEventJobAsync(eventId, endingDelay);
            }
            else
            {
                await scheduledJobsService.DeleteJobsAsync(eventId, true);
                await scheduledJobsService.CreateStartEventJobAsync(eventId, activationDelay);
                await scheduledJobsService.CreateEndEventJobAsync(eventId, endingDelay);
            }
        }

        #endregion
    }
}
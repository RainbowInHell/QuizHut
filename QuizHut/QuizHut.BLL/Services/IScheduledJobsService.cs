namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common.Enumerations;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class ScheduledJobsService : IScheduledJobsService
    {
        private readonly IDeletableEntityRepository<ScheduledJob> repository;
    
        private readonly IDeletableEntityRepository<Event> eventRepository;

        public ScheduledJobsService(
            IDeletableEntityRepository<ScheduledJob> repository,
            IDeletableEntityRepository<Event> eventRepository)
        {
            this.repository = repository;
            this.eventRepository = eventRepository;
        }

        public async Task CreateStartEventJobAsync(string eventId, TimeSpan activationDelay)
        {
            var job = new ScheduledJob()
            {
                JobId = Guid.NewGuid().ToString(),
                EventId = eventId,
                IsActivationJob = true,
            };

            await repository.AddAsync(job);
            await repository.SaveChangesAsync();
        }

        public async Task CreateEndEventJobAsync(string eventId, TimeSpan endingDelay)
        {
            var job = new ScheduledJob()
            {
                JobId = Guid.NewGuid().ToString(),
                EventId = eventId,
                IsActivationJob = false,
            };

            await repository.AddAsync(job);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteJobsAsync(string eventId, bool all, bool deleteActivationJobCondition = false)
        {
            var query = repository
                .All()
                .Where(x => x.EventId == eventId);

            if (!all)
            {
                query = query.Where(x => x.IsActivationJob == deleteActivationJobCondition);
            }

            var jobsIds = await query
                .Select(x => x.JobId)
                .ToListAsync();
        }

        public async Task SetStatusChangeJobAsync(string eventId, Status status)
        {
            var @event = await this.eventRepository
                .All()
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();

            var studentNames = await this.GetStudentsNamesByEventIdAsync(eventId);
            var adminUpdate = status == Status.Active ? "ActiveEventUpdate" : "EndedEventUpdate";
            var studentUpdate = status == Status.Active ? "NewActiveEventMessage" : "NewEndedEventMessage";

            if (@event.QuizId == null || @event.Status == status)
            {
                return;
            }

            @event.Status = status;
            eventRepository.Update(@event);
            await eventRepository.SaveChangesAsync();
        }

        private async Task<string[]> GetStudentsNamesByEventIdAsync(string id)
        {
            return await eventRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .SelectMany(x => x.EventsGroups.SelectMany(x => x.Group.StudentsGroups.Select(x => x.Student.UserName)))
                .ToArrayAsync();
        }
    }
}

namespace QuizHut.BLL.Services
{
    using Hangfire;

    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class ScheduledJobsService : IScheduledJobsService
    {
        private readonly IRepository<ScheduledJob> repository;

        private readonly IRepository<Event> eventRepository;

        private readonly IBackgroundJobClient backgroundJobClient;

        public ScheduledJobsService(
            IRepository<ScheduledJob> repository,
            IRepository<Event> eventRepository,
            IBackgroundJobClient backgroundJobClient)
        {
            this.repository = repository;
            this.eventRepository = eventRepository;
            this.backgroundJobClient = backgroundJobClient;
        }

        public async Task CreateStartEventJobAsync(string eventId, TimeSpan activationDelay)
        {
            var activationScheduleJobId = backgroundJobClient.Schedule(() => SetStatusChangeJobAsync(eventId, Status.Active), activationDelay);

            var job = new ScheduledJob()
            {
                JobId = activationScheduleJobId,
                EventId = eventId,
                IsActivationJob = true,
            };

            await repository.AddAsync(job);
            await repository.SaveChangesAsync();
        }

        public async Task CreateEndEventJobAsync(string eventId, TimeSpan endingDelay)
        {
            var activationScheduleJobId = backgroundJobClient.Schedule(() => SetStatusChangeJobAsync(eventId, Status.Ended), endingDelay);

            var job = new ScheduledJob()
            {
                JobId = activationScheduleJobId,
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

            var jobs = await query.ToListAsync();

            foreach (var job in jobs)
            {
                repository.Delete(job);
            }

            await repository.SaveChangesAsync();
        }

        public async Task SetStatusChangeJobAsync(string eventId, Status status)
        {
            var @event = await eventRepository
                .All()
                .Include(e => e.Quizzes)
                .Where(e => e.Id == eventId)
                .FirstOrDefaultAsync();

            if (@event.Quizzes.Count == 0 || @event.Status == status)
            {
                return;
            }

            @event.Status = status;
            eventRepository.Update(@event);
            await eventRepository.SaveChangesAsync();
        }
    }
}
namespace QuizHut.BLL.Services.Contracts
{
    public interface IScheduledJobsService
    {
        Task CreateStartEventJobAsync(string eventId, TimeSpan activationDelay);

        Task CreateEndEventJobAsync(string eventId, TimeSpan endingDelay);

        Task DeleteJobsAsync(string eventId, bool all, bool deleteActivationJobCondition = false);
    }
}
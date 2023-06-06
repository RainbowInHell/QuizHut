namespace QuizHut.BLL.Services.Contracts
{
    using QuizHut.DLL.Common;

    public interface IEventsService
    {
        Task<IList<T>> GetAllEventsAsync<T>(string creatorId = null, string searchCriteria = null, string searchText = null);

        Task<IList<T>> GetAllEventsByCreatorIdAndStatusAsync<T>(Status status, string creatorId, string searchText = null);

        Task<IList<T>> GetAllEventsFilteredByStatusAndGroupAsync<T>(Status status, string groupId, string creatorId = null);

        Task<IList<T>> GetAllEventsByStatusAndStudentIdAsync<T>(
            Status status,
            string studentId,
            string searchText = null);

        Task<IList<T>> GetAllEventsByGroupIdAsync<T>(string groupId);

        Task AssignQuizzesToEventAsync(IList<string> quizIds, string eventId);

        Task AssignGroupsToEventAsync(IList<string> groupIds, string eventId);

        Task<string> CreateEventAsync(string name, string activationDate, string activeFrom, string activeTo, string creatorId);

        Task UpdateEventAsync(string id, string name, string activationDate, string activeFrom, string ativeTo);

        Task DeleteQuizFromEventAsync(string eventId, string quizId);

        Task DeleteEventAsync(string eventId);

        Task SendEmailsToEventGroups(string eventId, string quizId);

        string GetTimeErrorMessage(string activeFrom, string activeTo, string activationDate, string oldActiveFrom = null);
    }
}
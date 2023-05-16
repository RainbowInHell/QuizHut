namespace QuizHut.BLL.Services.Contracts
{
    using QuizHut.DLL.Common;

    public interface IEventsService
    {
        Task<IList<T>> GetAllEventsAsync<T>(string creatorId = null, string searchCriteria = null, string searchText = null);

        Task<IList<T>> GetAllFilteredByStatusAndGroupAsync<T>(Status status, string groupId, string creatorId = null);

        Task<IList<T>> GetAllByGroupIdAsync<T>(string groupId);

        Task AssignQuizToEventAsync(string eventId, string quizId);

        Task AssignGroupsToEventAsync(IList<string> groupIds, string eventId);

        Task<string> CreateEventAsync(string name, string activationDate, string activeFrom, string activeTo, string creatorId);

        Task UpdateAsync(string id, string name, string activationDate, string activeFrom, string ativeTo);

        Task DeleteQuizFromEventAsync(string eventId, string quizId);

        Task DeleteAsync(string eventId);

        string GetTimeErrorMessage(string activeFrom, string activeTo, string activationDate);
    }
}
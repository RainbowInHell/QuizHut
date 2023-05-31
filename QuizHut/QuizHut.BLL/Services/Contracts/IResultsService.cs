namespace QuizHut.BLL.Services.Contracts
{
    public interface IResultsService
    {
        Task<IEnumerable<T>> GetAllResultsByEventAndGroupAsync<T>(string eventId, string groupId);

        Task<IEnumerable<T>> GetAllResultsByStudentIdAsync<T>(string studentId, string searchCriteria = null, string searchText = null);

        Task<bool> DoesParticipantHasResult(string participantId, string quizId);

        Task<string> CreateResultAsync(string studentId, string quizId);

        Task UpdateResultAsync(string id, decimal points, TimeSpan timeSpent);

        Task DeleteResultAsync(string id);
    }
}
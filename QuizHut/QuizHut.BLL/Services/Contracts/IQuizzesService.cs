namespace QuizHut.BLL.Services.Contracts
{
    public interface IQuizzesService
    {
        Task<IEnumerable<T>> GetAllQuizzesAsync<T>(
            string creatorId = null,
            string searchCriteria = null,
            string searchText = null,
            string categoryId = null);

        Task<IList<T>> GetAllByCategoryIdAsync<T>(string id);

        Task<string> GetQuizNameByIdAsync(string id);

        Task<IList<T>> GetUnAssignedToCategoryByCreatorIdAsync<T>(string categoryId, string creatorId);

        Task<IList<T>> GetAllUnAssignedToEventAsync<T>(string creatorId = null);

        Task<T> GetQuizByEventId<T>(string eventId);

        Task<string> GetQuizIdByPasswordAsync(string password);

        Task AssignQuizToEventAsync(string eventId, string quizId);

        Task<string> CreateQuizAsync(string name, string description, int? timer, string creatorId, string password);

        Task UpdateAsync(string id, string name, string description, int? timer, string password);

        Task DeleteByIdAsync(string id);

        Task DeleteEventFromQuizAsync(string eventId, string quizId);
    }
}
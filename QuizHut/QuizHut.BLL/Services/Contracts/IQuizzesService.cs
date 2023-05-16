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

        Task AssignQuizToEventAsync(string eventId, string quizId);

        Task DeleteEventFromQuizAsync(string eventId, string quizId);
    }
}
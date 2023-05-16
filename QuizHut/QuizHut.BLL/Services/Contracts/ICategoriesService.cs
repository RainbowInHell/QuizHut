namespace QuizHut.BLL.Services.Contracts
{
    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllCategories<T>(string creatorId, string searchText = null);

        Task AssignQuizzesToCategoryAsync(string id, IEnumerable<string> quizzesIds);

        Task<string> CreateCategoryAsync(string name, string creatorId);

        Task UpdateNameAsync(string id, string newName);

        Task DeleteAsync(string id);

        Task DeleteQuizFromCategoryAsync(string categoryId, string quizId);
    }
}
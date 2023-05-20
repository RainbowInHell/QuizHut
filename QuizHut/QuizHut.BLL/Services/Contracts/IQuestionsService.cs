namespace QuizHut.BLL.Services.Contracts
{
    public interface IQuestionsService
    {
        Task<IList<T>> GetAllQuestionsByQuizIdAsync<T>(string id);

        Task<string> CreateQuestionAsync(string quizId, string questionText);

        Task UpdateQuestionAsync(string id, string text);

        Task UpdateAllQuestionsInQuizNumerationAsync(string quizId);

        Task DeleteQuestionByIdAsync(string id);
    }
}
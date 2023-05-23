namespace QuizHut.BLL.Services.Contracts
{
    public interface IQuestionsService
    {
        Task<IList<T>> GetAllQuestionsByQuizIdAsync<T>(string id);

        Task<string> CreateQuestionAsync(string quizId, bool IsFullEvaluation, string questionText);

        Task UpdateQuestionAsync(string id, bool IsFullEvaluation, string text);

        Task UpdateAllQuestionsInQuizNumerationAsync(string quizId);

        Task DeleteQuestionByIdAsync(string id);
    }
}
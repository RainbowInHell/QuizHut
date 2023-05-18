namespace QuizHut.BLL.Services.Contracts
{
    public interface IQuestionsService
    {
        Task<IList<T>> GetAllByQuizIdAsync<T>(string id);

        Task<string> CreateQuestionAsync(string quizId, string questionText);

        Task Update(string id, string text);

        Task UpdateAllQuestionsInQuizNumeration(string quizId);

        Task DeleteQuestionByIdAsync(string id);
    }
}
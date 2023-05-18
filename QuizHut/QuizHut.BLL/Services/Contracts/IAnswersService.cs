namespace QuizHut.BLL.Services.Contracts
{
    public interface IAnswersService
    {
        Task<T> GetByIdAsync<T>(string id);

        Task CreateAnswerAsync(string answerText, bool isRightAnswer, string questionId);

        Task UpdateAsync(string id, string text, bool isRightAnswer);

        Task DeleteAsync(string id);
    }
}
namespace QuizHut.BLL.Services.Contracts
{
    public interface IAnswersService
    {
        Task CreateAnswerAsync(string answerText, bool isRightAnswer, string questionId);

        Task UpdateAnswerAsync(string id, string text, bool isRightAnswer);

        Task DeleteAnswerAsync(string id);
    }
}
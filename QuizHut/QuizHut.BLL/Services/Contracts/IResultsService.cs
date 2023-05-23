namespace QuizHut.BLL.Services.Contracts
{
    public interface IResultsService
    {
        Task<string> CreateResultAsync(string studentId, int points, int maxPoints, string quizId);
    }
}
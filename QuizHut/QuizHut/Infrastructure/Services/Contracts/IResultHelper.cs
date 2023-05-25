namespace QuizHut.Infrastructure.Services.Contracts
{
    using System.Collections.Generic;

    using QuizHut.Infrastructure.EntityViewModels.Questions;

    public interface IResultHelper
    {
        decimal CalculateResult(IList<QuestionViewModel> originalQuizQuestions, IList<AttemptedQuizQuestionViewModel> attemptedQuizQuestions);
    }
}
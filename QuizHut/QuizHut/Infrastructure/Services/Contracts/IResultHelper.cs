namespace QuizHut.Infrastructure.Services.Contracts
{
    using System.Collections.Generic;

    using QuizHut.Infrastructure.EntityViewModels.Questions;

    public interface IResultHelper
    {
        int CalculateResult(IList<QuestionViewModel> originalQuizQuestions, IList<AttemtedQuizQuestionViewModel> attemptedQuizQuestions);
    }
}
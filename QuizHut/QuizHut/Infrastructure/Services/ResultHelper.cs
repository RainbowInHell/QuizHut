namespace QuizHut.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.Services.Contracts;

    public class ResultHelper : IResultHelper
    {
        public decimal CalculateResult(IList<QuestionViewModel> originalQuizQuestions, IList<AttemptedQuizQuestionViewModel> attemptedQuizQuestions)
        {
            decimal totalPoints = 0;

            foreach (var attemptedQuestion in attemptedQuizQuestions)
            {
                var originalQuestion = originalQuizQuestions.FirstOrDefault(q => q.Id == attemptedQuestion.Id);

                if (originalQuestion != null)
                {
                    decimal questionPoints = 0;

                    if (attemptedQuestion.IsFullEvaluation)
                    {
                        bool isFullEvaluationMatch = attemptedQuestion.Answers.All(a =>
                            originalQuestion.Answers.Any(oa => oa.Id == a.Id && oa.IsRightAnswer == a.IsRightAnswerAssumption));

                        if (isFullEvaluationMatch)
                        {
                            questionPoints = 1;
                        }
                    }
                    else
                    {
                        int correctAnswerCount = originalQuestion.Answers.Count(a => a.IsRightAnswer);
                        int selectedAnswerCount = attemptedQuestion.Answers.Count(a => a.IsRightAnswerAssumption);
                        int selectedCorrectAnswerCount = attemptedQuestion.Answers.Count(a =>
                            originalQuestion.Answers.Any(oa => oa.Id == a.Id && oa.IsRightAnswer == a.IsRightAnswerAssumption && oa.IsRightAnswer));

                        if (selectedCorrectAnswerCount > 0)
                        {
                            if (correctAnswerCount == selectedCorrectAnswerCount && selectedAnswerCount == selectedCorrectAnswerCount)
                            {
                                questionPoints = 1;
                            }
                            else
                            {
                                decimal partialPoints = 1m / originalQuestion.Answers.Count();
                                questionPoints = selectedCorrectAnswerCount * partialPoints;
                            }
                        }
                    }

                    totalPoints += questionPoints;
                }
            }

            return Math.Round(totalPoints, 2);
        }
    }
}
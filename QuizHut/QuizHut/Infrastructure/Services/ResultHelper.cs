namespace QuizHut.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.Services.Contracts;

    public class ResultHelper : IResultHelper
    {
        //public int CalculateResult(
        //    IList<QuestionViewModel> originalQuizQuestions,
        //    IList<AttemtedQuizQuestionViewModel> attemptedQuizQuestions)
        //{
        //    var totalPoints = 0;
        //    foreach (var question in originalQuizQuestions)
        //    {
        //        var points = 0;
        //        var correspondingAttendedQuestion = attemptedQuizQuestions.FirstOrDefault(x => x.Id == question.Id);
        //        var corectAnswersInQuestion = question.Answers.Where(x => x.IsRightAnswer).Count();
        //        var originalAnswers = question.Answers;
        //        foreach (var answer in originalAnswers)
        //        {
        //            var correspondingAnswerAttempt = correspondingAttendedQuestion.Answers.FirstOrDefault(x => x.Id == answer.Id);
        //            if (answer.IsRightAnswer == false && correspondingAnswerAttempt.IsRightAnswerAssumption == false)
        //            {
        //                continue;
        //            }
        //            else if (answer.IsRightAnswer != correspondingAnswerAttempt.IsRightAnswerAssumption)
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                points++;
        //            }
        //        }

        //        if (points == corectAnswersInQuestion)
        //        {
        //            totalPoints++;
        //        }
        //    }

        //    return totalPoints;
        //}

        //public int CalculateResult(
        //    IList<QuestionViewModel> originalQuizQuestions,
        //    IList<AttemtedQuizQuestionViewModel> attemptedQuizQuestions)
        //{
        //    var totalPoints = 0;
        //    foreach (var question in originalQuizQuestions)
        //    {
        //        var correspondingAttendedQuestion = attemptedQuizQuestions.FirstOrDefault(x => x.Id == question.Id);
        //        if (correspondingAttendedQuestion == null)
        //        {
        //            continue; // Skip if the question was not attempted
        //        }

        //        var correctAnswersInQuestion = question.Answers.Count(x => x.IsRightAnswer);
        //        var originalAnswers = question.Answers;
        //        var correctAnswersSelected = 0;

        //        foreach (var answer in originalAnswers)
        //        {
        //            var correspondingAnswerAttempt = correspondingAttendedQuestion.Answers.FirstOrDefault(x => x.Id == answer.Id);
        //            if (correspondingAnswerAttempt == null)
        //            {
        //                continue; // Skip if the answer was not selected in the attempted question
        //            }

        //            if (answer.IsRightAnswer == correspondingAnswerAttempt.IsRightAnswerAssumption)
        //            {
        //                correctAnswersSelected++; // Increment count for each correctly selected answer
        //            }
        //        }

        //        if (correctAnswersSelected == correctAnswersInQuestion)
        //        {
        //            totalPoints++;
        //        }
        //    }

        //    return totalPoints;
        //}
        public int CalculateResult(
            IList<QuestionViewModel> originalQuizQuestions,
            IList<AttemtedQuizQuestionViewModel> attemptedQuizQuestions)
        {
            int totalQuestions = originalQuizQuestions.Count;
            int correctAnswers = 0;

            foreach (var attemptedQuestion in attemptedQuizQuestions)
            {
                var originalQuestion = originalQuizQuestions.FirstOrDefault(q => q.Id == attemptedQuestion.Id);

                if (originalQuestion != null)
                {
                    foreach (var attemptedAnswer in attemptedQuestion.Answers)
                    {
                        var originalAnswer = originalQuestion.Answers.FirstOrDefault(a => a.Id == attemptedAnswer.Id);

                        if (originalAnswer != null && originalAnswer.IsRightAnswer && attemptedAnswer.IsRightAnswerAssumption)
                        {
                            correctAnswers++;
                        }
                    }
                }
            }

            return correctAnswers;
        }
    }
}
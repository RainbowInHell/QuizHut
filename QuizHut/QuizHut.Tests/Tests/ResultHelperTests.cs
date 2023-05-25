namespace QuizHut.Tests.Tests
{
    using QuizHut.Infrastructure.EntityViewModels.Answers;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.Services;
    
    using Xunit;

    public class ResultHelperTests
    {
        private readonly ResultHelper _resultHelper;

        public ResultHelperTests()
        {
            _resultHelper = new ResultHelper();
        }

        [Fact]
        public void CalculateResult_FullEvaluation_AllAnswersCorrect_Returns1()
        {
            // Arrange 
            var originalQuizQuestions = new List<QuestionViewModel>
            {
                new QuestionViewModel
                {
                    Id = "1",
                    Answers = new List<AnswerViewModel>
                    {
                        new AnswerViewModel { Id = "1", IsRightAnswer = true },
                        new AnswerViewModel { Id = "2", IsRightAnswer = true }
                    }
                }
            };

            var attemptedQuizQuestions = new List<AttemptedQuizQuestionViewModel>
            {
                new AttemptedQuizQuestionViewModel
                {
                    Id = "1",
                    IsFullEvaluation = true,
                    Answers = new List<AttemptedQuizAnswerViewModel>
                    {
                        new AttemptedQuizAnswerViewModel { Id = "1", IsRightAnswerAssumption = true },
                        new AttemptedQuizAnswerViewModel { Id = "2", IsRightAnswerAssumption = true }
                    }
                }
            };

            // Act 
            decimal result = _resultHelper.CalculateResult(originalQuizQuestions, attemptedQuizQuestions);

            // Assert 
            Assert.Equal(1, result);
        }

        [Fact]
        public void CalculateResult_FullEvaluation_NotAllAnswersCorrect_Returns0()
        {
            // Arrange 
            var originalQuizQuestions = new List<QuestionViewModel>
            {
                new QuestionViewModel
                {
                    Id = "1",
                    Answers = new List<AnswerViewModel>
                    {
                        new AnswerViewModel { Id = "1", IsRightAnswer = true },
                        new AnswerViewModel { Id = "2", IsRightAnswer = true }
                    }
                }
            };

            var attemptedQuizQuestions = new List<AttemptedQuizQuestionViewModel>
            {
                new AttemptedQuizQuestionViewModel
                {
                    Id = "1",
                    IsFullEvaluation = true,
                    Answers = new List<AttemptedQuizAnswerViewModel>
                    {
                        new AttemptedQuizAnswerViewModel { Id = "1", IsRightAnswerAssumption = true },
                        new AttemptedQuizAnswerViewModel { Id = "2", IsRightAnswerAssumption = false }
                    }
                }
            };

            // Act 
            decimal result = _resultHelper.CalculateResult(originalQuizQuestions, attemptedQuizQuestions);

            // Assert 
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateResult_PartialEvaluation_AllAnswersCorrect_Returns1()
        {
            // Arrange 
            var originalQuizQuestions = new List<QuestionViewModel>
            {
                new QuestionViewModel
                {
                    Id = "1",
                    Answers = new List<AnswerViewModel>
                    {
                        new AnswerViewModel { Id = "1", IsRightAnswer = true },
                        new AnswerViewModel { Id = "2", IsRightAnswer = true }
                    }
                }
            };

            var attemptedQuizQuestions = new List<AttemptedQuizQuestionViewModel>
            {
                new AttemptedQuizQuestionViewModel
                {
                    Id = "1",
                    IsFullEvaluation = false,
                    Answers = new List<AttemptedQuizAnswerViewModel>
                    {
                        new AttemptedQuizAnswerViewModel { Id = "1", IsRightAnswerAssumption = true },
                        new AttemptedQuizAnswerViewModel { Id = "2", IsRightAnswerAssumption = true }
                    }
                }
            };

            // Act 
            decimal result = _resultHelper.CalculateResult(originalQuizQuestions, attemptedQuizQuestions);

            // Assert 
            Assert.Equal(1, result);
        }

        [Fact]
        public void CalculateResult_PartialEvaluation_SomeAnswersCorrect_ReturnsPartialPoints()
        {
            // Arrange 
            var originalQuizQuestions = new List<QuestionViewModel>
            {
                new QuestionViewModel
                {
                    Id = "1",
                    Answers = new List<AnswerViewModel>
                    {
                        new AnswerViewModel { Id = "1", IsRightAnswer = true },
                        new AnswerViewModel { Id = "2", IsRightAnswer = true }
                    }
                }
            };

            var attemptedQuizQuestions = new List<AttemptedQuizQuestionViewModel>
            {
                new AttemptedQuizQuestionViewModel
                {
                    Id = "1",
                    IsFullEvaluation = false,
                    Answers = new List<AttemptedQuizAnswerViewModel>
                    {
                        new AttemptedQuizAnswerViewModel { Id = "1", IsRightAnswerAssumption = true },
                        new AttemptedQuizAnswerViewModel { Id = "2", IsRightAnswerAssumption = false }
                    }
                }
            };

            // Act 
            decimal result = _resultHelper.CalculateResult(originalQuizQuestions, attemptedQuizQuestions);

            // Assert 
            Assert.Equal(0.5m, result);
        }
    }
}
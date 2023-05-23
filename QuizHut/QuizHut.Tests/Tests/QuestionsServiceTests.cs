namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class QuestionsServiceTests : SetUpTests
    {
        private IQuestionsService Service => ServiceProvider.GetRequiredService<IQuestionsService>();

        [Fact]
        public async Task GetAllQuestionsByQuizIdAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            var quizId = await CreateQuizAsync();
            var firstQuestionId = await Service.CreateQuestionAsync(quizId, false, "First Question");
            var secondQuestionId = await Service.CreateQuestionAsync(quizId, false, "Second Question");

            var firstModel = new QuestionViewModel()
            {
                Id = firstQuestionId,
                Text = "First Question",
                Number = 1,
            };

            var secondModel = new QuestionViewModel()
            {
                Id = secondQuestionId,
                Text = "Second Question",
                Number = 2,
            };

            //Act
            var resultModelCollection = await Service.GetAllQuestionsByQuizIdAsync<QuestionViewModel>(quizId);

            //Arrange
            Assert.Equal(firstModel.Id, resultModelCollection.First().Id);
            Assert.Equal(firstModel.Text, resultModelCollection.First().Text);
            Assert.Equal(firstModel.Answers.Count, resultModelCollection.First().Answers.Count);
            Assert.Equal(secondModel.Id, resultModelCollection.Last().Id);
            Assert.Equal(secondModel.Text, resultModelCollection.Last().Text);
            Assert.Equal(secondModel.Answers.Count, resultModelCollection.Last().Answers.Count);
            Assert.Equal(2, resultModelCollection.Count());
        }

        [Fact]
        public async Task CreateQuestionAsync_ShouldCreateNewQuestionInDb()
        {
            //Arrange
            var quizId = await CreateQuizAsync();

            //Act
            var newQuestionId = await Service.CreateQuestionAsync(quizId, false, "First question text");
            var questionsCount = DbContext.Questions.ToArray().Count();
            
            //Assert
            Assert.Equal(1, questionsCount);
            Assert.NotNull(await DbContext.Questions.FirstOrDefaultAsync(x => x.Id == newQuestionId));
        }

        [Fact]
        public async Task UpdateQuestionAsync_ShouldUpdateQuestionCorrectly()
        {
            //Arrange
            var quizId = await CreateQuizAsync();
            var questionId = await CreateAndAddQuestionToQuiz(quizId, 1, "text");
            
            //Act
            await Service.UpdateQuestionAsync(questionId, false, "Updated text");

            //Assert
            var question = await DbContext.Questions.FindAsync(questionId);
            Assert.Equal("Updated text", question.Text);
        }

        [Fact]
        public async Task UpdateAllQuestionsInQuizNumerationAsync_ShouldUpdateNumerationOfQuestionsCorrectly()
        {
            //Arrange
            var quizId = await CreateQuizAsync();
            var firstQuestionId = await CreateAndAddQuestionToQuiz(quizId, 1, "text");
            var secondQuestionId = await CreateAndAddQuestionToQuiz(quizId, 2, "text");
            var thirdQuestionId = await CreateAndAddQuestionToQuiz(quizId, 3, "text");

            await DeleteQuestionAsync(secondQuestionId, quizId);

            //Act
            await Service.UpdateAllQuestionsInQuizNumerationAsync(quizId);

            //Assert
            var firstQuestion = await DbContext.Questions.FirstOrDefaultAsync(x => x.Id == firstQuestionId);
            var thirdQuestion = await DbContext.Questions.FirstOrDefaultAsync(x => x.Id == thirdQuestionId);

            Assert.Equal(1, firstQuestion.Number);
            Assert.Equal(2, thirdQuestion.Number);
        }

        [Fact]
        public async Task DeleteQuestionByIdAsync_ShouldDeleteCorrectly()
        {
            //Arrangt
            var quizId = await CreateQuizAsync();
            var questionId = await CreateAndAddQuestionToQuiz(quizId, 1, "text");
            
            //Act
            await Service.DeleteQuestionByIdAsync(questionId);

            //Assert
            var questionsCount = DbContext.Questions.ToArray().Count();
            Assert.Equal(0, questionsCount);
        }

        private async Task<string> CreateAndAddQuestionToQuiz(string quizId, int questionNumber, string text)
        {
            var question = new Question
            {
                Number = questionNumber,
                Text = text,
                QuizId = quizId,
            };

            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            
            quiz.Questions.Add(question);
            DbContext.Update(quiz);
            
            await DbContext.SaveChangesAsync();
            DbContext.Entry(question).State = EntityState.Detached;

            return question.Id;
        }

        private async Task<string> CreateQuizAsync()
        {
            var quiz = new Quiz() { Name = "Test Quiz", CreatorId = "asd", Description = "des", Password = "asd" };
            
            await DbContext.Quizzes.AddAsync(quiz);
            await DbContext.SaveChangesAsync();

            return quiz.Id;
        }

        private async Task DeleteQuestionAsync(string questionId, string quizId)
        {
            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            var question = await DbContext.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

            quiz.Questions.Remove(question);
            DbContext.Quizzes.Update(quiz);
            
            DbContext.Questions.Remove(question);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(quiz).State = EntityState.Detached;
        }
    }
}
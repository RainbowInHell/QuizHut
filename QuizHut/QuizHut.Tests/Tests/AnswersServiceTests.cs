namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class AnswersServiceTests : SetUpTests
    {
        private IAnswersService Service => ServiceProvider.GetRequiredService<IAnswersService>();

        [Fact]
        public async Task CreateAnswerAsync_ShouldCreateNewAnswer()
        {
            //Arrange
            var questionId = Guid.NewGuid().ToString();

            //Act
            await Service.CreateAnswerAsync("answer", false, questionId);

            //Assert
            var answer = await DbContext.Answers.FirstOrDefaultAsync();
            var answersCount = DbContext.Answers.ToArray().Count();

            Assert.Equal(1, answersCount);
            Assert.Equal("answer", answer.Text);
            Assert.Equal(questionId, answer.QuestionId);
            Assert.False(answer.IsRightAnswer);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCorrectly()
        {
            //Arrange
            var answer = await CreateAnswerAsync();

            //Act
            await Service.UpdateAnswerAsync(answer.Id, "First test answer", false);
            
            //Assert
            var updatedAnswer = await DbContext.Answers.FirstOrDefaultAsync(x => x.Id == answer.Id);

            Assert.Equal("First test answer", updatedAnswer.Text);
            Assert.False(updatedAnswer.IsRightAnswer);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCorrectly()
        {
            //Assrange
            var answer = await CreateAnswerAsync();

            //Act
            await Service.DeleteAnswerAsync(answer.Id);

            //Assert
            var answersCount = DbContext.Answers.ToArray().Count();
            var deletedAnswer = await DbContext.Answers.FirstOrDefaultAsync(x => x.Id == answer.Id);
            
            Assert.Equal(0, answersCount);
            Assert.Null(deletedAnswer);
        }

        private async Task<Answer> CreateAnswerAsync()
        {            
            var answer = new Answer()
            {
                Text = "First answer",
                QuestionId = Guid.NewGuid().ToString(),
                IsRightAnswer = true,
            };
            
            await DbContext.Answers.AddAsync(answer);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(answer).State = EntityState.Detached;

            return answer;
        }
    }
}
namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class QuizzesServiceTests : SetUpTests
    {
        private IQuizzesService Service => ServiceProvider.GetRequiredService<IQuizzesService>();

        [Fact]
        public async Task GetQuizzesByCategoryIdAsyncShouldReturnCorrectModelCollection()
        {
            var creatorId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            var quizId = await CreateQuizAsync("First quiz", creatorId, "testquizpass");
            
            await AddQuizToCategoryAsync(categoryId, quizId);
            await CreateQuizAsync("Second quiz", password: "asd");

            var model = new QuizAssignViewModel()
            {
                Id = quizId,
                Name = "First quiz",
                CreatorId = creatorId,
                IsAssigned = false,
            };

            var resultModelCollection = await Service.GetQuizzesByCategoryIdAsync<QuizAssignViewModel>(categoryId);

            Assert.IsAssignableFrom<IList<QuizAssignViewModel>>(resultModelCollection);
            Assert.Equal(model.Id, resultModelCollection.First().Id);
            Assert.Equal(model.Name, resultModelCollection.First().Name);
            Assert.Equal(model.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
        }

        //[Fact]
        //public async Task GetUnAssignedQuizzesToCategoryAsyncShouldReturnCorrectModelCollection()
        //{
        //    var creatorId = Guid.NewGuid().ToString();
        //    var firstQuizId = await CreateQuizAsync("First quiz", creatorId, "testquizpass");

        //    var secondQuizId = await CreateQuizAsync("Second quiz", creatorId, "123");
        //    var categoryId = Guid.NewGuid().ToString();
        //    await AddQuizToCategoryAsync(categoryId, secondQuizId);

        //    var model = new QuizAssignViewModel()
        //    {
        //        Id = firstQuizId,
        //        Name = "First quiz",
        //        CreatorId = creatorId,
        //        IsAssigned = false,
        //    };

        //    var resultModelCollection = await Service.GetUnAssignedQuizzesToCategoryByCreatorIdAsync<QuizAssignViewModel>(categoryId, creatorId);

        //    Assert.IsAssignableFrom<IList<QuizAssignViewModel>>(resultModelCollection);
        //    Assert.Equal(model.Id, resultModelCollection.First().Id);
        //    Assert.Equal(model.Name, resultModelCollection.First().Name);
        //    Assert.Equal(model.CreatorId, resultModelCollection.First().CreatorId);
        //    Assert.False(resultModelCollection.First().IsAssigned);
        //}

        [Fact]
        public async Task GetUnAssignedQuizzesToEventAsync_ShouldReturnCorrectModelCollection()
        {
            var creatorId = Guid.NewGuid().ToString();
            var firstQuizId = await CreateQuizAsync("First quiz", creatorId, "123");

            var secondQuizId = await CreateQuizAsync("Second quiz", creatorId, "123");

            var firstModel = new QuizAssignViewModel()
            {
                Id = firstQuizId,
                Name = "First quiz",
                CreatorId = creatorId,
                IsAssigned = false,
            };

            var secondModel = new QuizAssignViewModel()
            {
                Id = secondQuizId,
                Name = "Second quiz",
                CreatorId = creatorId,
                IsAssigned = false,
            };

            var resultModelCollection = await Service.GetUnAssignedQuizzesToEventAsync<QuizAssignViewModel>();

            Assert.Equal(2, resultModelCollection.Count());
            Assert.IsAssignableFrom<IList<QuizAssignViewModel>>(resultModelCollection);

            Assert.Equal(firstModel.Id, resultModelCollection.Last().Id);
            Assert.Equal(firstModel.Name, resultModelCollection.Last().Name);
            Assert.Equal(firstModel.CreatorId, resultModelCollection.Last().CreatorId);
            Assert.False(resultModelCollection.Last().IsAssigned);

            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
        }

        [Fact]
        public async Task GetUnAssignedQuizzesToEventAsync_ShouldReturnCorrectModelCollectionIfCreatorIdIsPassed()
        {
            var creatorId = Guid.NewGuid().ToString();
            var firstQuizId = await CreateQuizAsync("First quiz", creatorId, "123");

            var firstModel = new QuizAssignViewModel()
            {
                Id = firstQuizId,
                Name = "First quiz",
                CreatorId = creatorId,
                IsAssigned = false,
            };

            var resultModelCollection = await Service.GetUnAssignedQuizzesToEventAsync<QuizAssignViewModel>(creatorId);

            Assert.Single(resultModelCollection);
            Assert.IsAssignableFrom<IList<QuizAssignViewModel>>(resultModelCollection);

            Assert.Equal(firstModel.Id, resultModelCollection.First().Id);
            Assert.Equal(firstModel.Name, resultModelCollection.First().Name);
            Assert.Equal(firstModel.CreatorId, resultModelCollection.First().CreatorId);
            Assert.False(resultModelCollection.First().IsAssigned);
        }

        [Fact]
        public async Task GetQuizIdByPasswordAsyncShouldReturnCorrectQuizId()
        {
            //Arrange
            var quizId = await CreateQuizAsync("Quiz", null, "testpassword");

            //Act
            var resultQuiz = await Service.GetQuizByPasswordAsync<QuizListViewModel>("testpassword");

            //Assert
            Assert.NotNull(resultQuiz);
            Assert.Equal(quizId, resultQuiz.Id);
        }

        [Fact]
        public async Task CreateQuizAsync_ShouldCreateCorrectly()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            
            //Act
            var quizId = await Service.CreateQuizAsync("Quiz", "description", 30, creatorId, "123456789");

            //Arrange
            var quiz = await GetQuizAsync(quizId);

            Assert.NotNull(quiz);
            Assert.Equal("Quiz", quiz.Name);
            Assert.Equal("description", quiz.Description);
            Assert.Equal(30, quiz.Timer);
            Assert.Equal(creatorId, quiz.CreatorId);
            Assert.Equal("123456789", quiz.Password);
        }

        [Fact]
        public async Task UpdateQuizAsync_ShouldUpdateCorrectly()
        {
            //Arrange
            var quizId = await CreateQuizAsync("Test quiz", password: "asdasd");

            //Act
            await Service.UpdateQuizAsync(quizId, "First Quiz", "Description", 32, "6543211");

            //Assert
            var quizAfterUpdate = await GetQuizAsync(quizId);

            Assert.Equal("First Quiz", quizAfterUpdate.Name);
            Assert.Equal("Description", quizAfterUpdate.Description);
            Assert.Equal(32, quizAfterUpdate.Timer);
            Assert.Equal("6543211", quizAfterUpdate.Password);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteQuizCorrectly()
        {
            //Arrange
            var quizId = await CreateQuizAsync("Quiz", null, "testpassword");

            //Act
            await Service.DeleteQuizAsync(quizId);

            //Assert
            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync();

            Assert.Null(quiz);
        }

        private async Task<string> CreateQuizAsync(string name, string creatorId = null, string password = null)
        {
            var quiz = new Quiz
            {
                Name = name,
                Description = "",
                Timer = null,
                CreatorId = creatorId ?? Guid.NewGuid().ToString(),
                Password = password
            };

            await DbContext.Quizzes.AddAsync(quiz);
            await DbContext.SaveChangesAsync();

            DbContext.Quizzes.Update(quiz);
            await DbContext.SaveChangesAsync();

            DbContext.Entry(quiz).State = EntityState.Detached;

            return quiz.Id;
        }

        private async Task<Quiz> GetQuizAsync(string id)
        {
            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == id);
            
            DbContext.Entry<Quiz>(quiz).State = EntityState.Detached;
         
            return quiz;
        }

        private async Task AddQuizToCategoryAsync(string categoryId, string quizId)
        {
            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            quiz.CategoryId = categoryId;
            
            DbContext.Update(quiz);
            await DbContext.SaveChangesAsync();
         
            DbContext.Entry<Quiz>(quiz).State = EntityState.Detached;
        }
    }
}
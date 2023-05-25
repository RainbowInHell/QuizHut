namespace QuizHut.Tests.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Tests.Configuration;
    
    using Xunit;

    public class CategoriesServiceTests : SetUpTests
    {
        private ICategoriesService Service => ServiceProvider.GetRequiredService<ICategoriesService>();

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnCorrectModelCollection()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();
            var firstCategory = await CreateCategoryAsync("Category 1", creatorId);
            var secondCategory = await CreateCategoryAsync("Category 2", creatorId);

            var firstModel = new CategoryViewModel()
            {
                Name = firstCategory.Name,
                Id = firstCategory.Id,
                QuizzesCount = firstCategory.Quizzes.Count().ToString(),
                CreatedOn = firstCategory.CreatedOn,
            };

            var secondModel = new CategoryViewModel()
            {
                Name = secondCategory.Name,
                Id = secondCategory.Id,
                QuizzesCount = secondCategory.Quizzes.Count().ToString(),
                CreatedOn = secondCategory.CreatedOn,
            };

            //Act
            var resultModelCollection = await Service.GetAllCategories<CategoryViewModel>(creatorId);

            //Assert
            Assert.Equal(firstModel.Id, resultModelCollection.Last().Id);
            Assert.Equal(firstModel.Name, resultModelCollection.Last().Name);
            Assert.Equal(firstModel.QuizzesCount, resultModelCollection.Last().QuizzesCount);
            Assert.Equal(firstModel.CreatedOn, resultModelCollection.Last().CreatedOn);
            Assert.Equal(secondModel.Id, resultModelCollection.First().Id);
            Assert.Equal(secondModel.Name, resultModelCollection.First().Name);
            Assert.Equal(secondModel.QuizzesCount, resultModelCollection.First().QuizzesCount);
            Assert.Equal(secondModel.CreatedOn, resultModelCollection.First().CreatedOn);
            Assert.Equal(2, resultModelCollection.Count());
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldCreateNewCategory()
        {
            //Arrange
            var creatorId = Guid.NewGuid().ToString();

            //Act
            var newCategoryId = await Service.CreateCategoryAsync(name: "Second category", creatorId);
            
            //Assert
            var categoriesCount = DbContext.Categories.ToArray().Count();
            var newCategory = await DbContext.Categories.FirstOrDefaultAsync(x => x.Id == newCategoryId);

            Assert.Equal(1, categoriesCount);
            Assert.Equal("Second category", newCategory.Name);
            Assert.Equal(creatorId, newCategory.CreatorId);
        }

        [Fact]
        public async Task UpdateCategoryNameAsync_ShouldUpdateCorrectly()
        {
            //Arrange
            var category = await CreateCategoryAsync("Category");

            //Act
            await Service.UpdateCategoryNameAsync(category.Id, "First test category");
            
            //Assert
            var updatedCategory = await DbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            Assert.Equal("First test category", updatedCategory.Name);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteCorrectly()
        {
            //Arrange
            var firstCategory = await CreateCategoryAsync("Category 1");
            
            //Act
            await Service.DeleteCategoryAsync(firstCategory.Id);

            //Assert
            var categoriesCount = DbContext.Categories.ToArray().Count();
            var category = await DbContext.Categories.FindAsync(firstCategory.Id);
            
            Assert.Equal(0, categoriesCount);
            Assert.Null(category);
        }

        [Fact]
        public async Task AssignQuizzesToCategoryAsync_ShouldAssignQuizzesCorrectly()
        {
            //Arrange
            var firstQuizId = CreateQuiz(name: "First quiz");
            var secondQuizId = CreateQuiz(name: "Second quiz");
            var firstCategory = await CreateCategoryAsync("Category 1");

            var quizzesIdList = new List<string>() { firstQuizId, secondQuizId };
            
            //Act
            await Service.AssignQuizzesToCategoryAsync(firstCategory.Id, quizzesIdList);
            
            //Assert
            var categoryQuizzesIds = await DbContext
                .Categories
                .Where(x => x.Id == firstCategory.Id)
                .Select(x => x.Quizzes.Select(x => x.Id))
                .FirstOrDefaultAsync();

            Assert.Equal(2, categoryQuizzesIds.Count());
            Assert.Contains(firstQuizId, categoryQuizzesIds);
            Assert.Contains(secondQuizId, categoryQuizzesIds);
        }

        [Fact]
        public async Task DeleteQuizFromCategoryAsync_ShouldUnAssignQuizCorrectly()
        {
            //Arrange
            var firstCategory = await CreateCategoryAsync("Category 2");

            var quizId = CreateQuiz(name: "quiz");
            await AssignQuizToFirstCategoryAsync(quizId);
            
            //Act
            await Service.DeleteQuizFromCategoryAsync(firstCategory.Id, quizId);
            
            //Assert
            var categoryQuizzesIds = await DbContext
                .Categories
                .Where(x => x.Id == firstCategory.Id)
                .Select(x => x.Quizzes.Select(x => x.Id))
                .FirstOrDefaultAsync();

            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);

            Assert.Empty(categoryQuizzesIds);
            Assert.Null(quiz.CategoryId);
        }

        private async Task<Category> CreateCategoryAsync(string name, string creatorId = null)
        {
            if (creatorId == null)
            {
                creatorId = Guid.NewGuid().ToString();
            }

            var category = new Category()
            {
                Name = name,
                CreatorId = creatorId,
            };

            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(category).State = EntityState.Detached;
            
            return category;
        }

        private string CreateQuiz(string name)
        {
            var creatorId = Guid.NewGuid().ToString();

            var quiz = new Quiz()
            {
                Name = name,
                Description = "",
                Password = "",
                CreatorId = creatorId,
            };

            DbContext.Quizzes.Add(quiz);
            DbContext.SaveChanges();
            DbContext.Entry(quiz).State = EntityState.Detached;
            
            return quiz.Id;
        }

        private async Task AssignQuizToFirstCategoryAsync(string quizId)
        {
            var category = await CreateCategoryAsync("Category");
            var quiz = await DbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            
            category.Quizzes.Add(quiz);
            
            await DbContext.SaveChangesAsync();
            
            DbContext.Entry(category).State = EntityState.Detached;
            DbContext.Entry(quiz).State = EntityState.Detached;
        }
    }
}
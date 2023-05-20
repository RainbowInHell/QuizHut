namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class QuizzesService : IQuizzesService
    {
        private readonly IRepository<Quiz> quizRepository;
        
        private readonly IExpressionBuilder expressionBuilder;

        public QuizzesService(
            IRepository<Quiz> quizRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.quizRepository = quizRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<IEnumerable<T>> GetAllQuizzesAsync<T>(
            string creatorId = null,
            string searchCriteria = null,
            string searchText = null,
            string categoryId = null)
        {
            var query = quizRepository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            if (categoryId != null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            var emptyNameInput = searchText == null && searchCriteria == "Name";
            if (searchCriteria != null && !emptyNameInput)
            {
                var filter = expressionBuilder.GetExpression<Quiz>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IList<T>> GetQuizzesByCategoryIdAsync<T>(string id)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == id)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IList<T>> GetUnAssignedQuizzesToCategoryByCreatorIdAsync<T>(string categoryId, string creatorId)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CreatorId == creatorId && x.CategoryId != categoryId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IList<T>> GetUnAssignedQuizzesToEventAsync<T>(string creatorId = null)
        {
            var query = quizRepository
                   .AllAsNoTracking()
                   .Where(x => x.EventId == null);

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            return await query.OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IList<T>> GetQuizzesByEventId<T>(string eventId)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.EventId == eventId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<string> GetQuizIdByPasswordAsync(string password)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.Password == password)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<string> CreateQuizAsync(string name, string description, int? timer, string creatorId, string password)
        {
            var quiz = new Quiz
            {
                Name = name,
                Description = description,
                Timer = timer,
                CreatorId = creatorId,
                Password = password
            };

            await quizRepository.AddAsync(quiz);
            await quizRepository.SaveChangesAsync();

            return quiz.Id;
        }

        public async Task UpdateQuizAsync(string id, string name, string description, int? timer, string password)
        {
            var quiz = await quizRepository
               .All()
               .FirstOrDefaultAsync(x => x.Id == id);

            if (quiz.Password != password)
            {
                quiz.Password = password;
            }

            quiz.Name = name;
            quiz.Description = description;
            quiz.Timer = timer;

            quizRepository.Update(quiz);
            await quizRepository.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(string id)
        {
            var quiz = await quizRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            quizRepository.Delete(quiz);

            await quizRepository.SaveChangesAsync();
        }
    }
}
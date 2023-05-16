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

        public async Task<IList<T>> GetAllByCategoryIdAsync<T>(string id)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == id)
                .To<T>()
                .ToListAsync();
        }

        public async Task<string> GetQuizNameByIdAsync(string id)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<T>> GetUnAssignedToCategoryByCreatorIdAsync<T>(string categoryId, string creatorId)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CreatorId == creatorId && x.CategoryId != categoryId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IList<T>> GetAllUnAssignedToEventAsync<T>(string creatorId = null)
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

        public async Task<T> GetQuizByEventId<T>(string eventId)
        {
            return await quizRepository
                .All()
                .Where(x => x.EventId == eventId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task AssignQuizToEventAsync(string eventId, string quizId)
        {
            var quiz = await quizRepository
                .All()
                .Where(x => x.Id == quizId)
                .FirstOrDefaultAsync();

            quiz.EventId = eventId;
            quizRepository.Update(quiz);
            
            await quizRepository.SaveChangesAsync();
        }

        public async Task DeleteEventFromQuizAsync(string eventId, string quizId)
        {
            var quiz = await quizRepository
                .All()
                .Where(x => x.Id == quizId)
                .FirstOrDefaultAsync();

            quiz.EventId = null;
            quizRepository.Update(quiz);

            await quizRepository.SaveChangesAsync();
        }
    }
}
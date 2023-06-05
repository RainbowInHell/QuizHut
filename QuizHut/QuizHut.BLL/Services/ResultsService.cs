namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class ResultsService : IResultsService
    {
        private readonly IRepository<Result> repository;

        private readonly IRepository<Quiz> quizRepository;

        private readonly IExpressionBuilder expressionBuilder;

        public ResultsService(
            IRepository<Result> repository,
            IRepository<Quiz> quizRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.repository = repository;
            this.quizRepository = quizRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<IEnumerable<T>> GetAllResultsByEventAndGroupAsync<T>(string eventId, string groupId)
        {
            return await repository
                .AllAsNoTracking()
                .Where(x => x.Quiz.EventId == eventId && x.Student.StudentsInGroups.Any(s => s.Group.Id == groupId))
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllResultsByStudentIdAsync<T>(string studentId, string searchCriteria = null, string searchText = null)
        {
            var query = repository
                .AllAsNoTracking()
                .Include(x => x.Quiz)
                .ThenInclude(x => x.Event)
                .Where(x => x.StudentId == studentId)
                .To<T>();

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<T>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllResultsByStudentIdAndQuizIdAsync<T>(string studentId, string quizId)
        {
            return await repository
                .AllAsNoTracking()
                .Include(x => x.Quiz)
                .Where(x => x.StudentId == studentId && x.QuizId == quizId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<bool> DoesParticipantHasResult(string participantId, string quizId)
        {
            var res =  await repository
             .AllAsNoTracking()
             .Where(x => x.StudentId == participantId && x.QuizId == quizId)
             .FirstOrDefaultAsync();

            return res != null;
        }

        public async Task<string> CreateResultAsync(string studentId, string quizId)
        {
            var quiz = await quizRepository
                .All()
                .Where(x => x.Id == quizId)
                .FirstOrDefaultAsync();

            var result = new Result()
            {
                StudentId = studentId,
                QuizId = quizId,
                MaxPoints = quiz.Questions.Count
            };

            await repository.AddAsync(result);
            await repository.SaveChangesAsync();

            return result.Id;
        }

        public async Task UpdateResultAsync(string id, decimal points, TimeSpan timeSpent)
        {
            var result = await repository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            result.Points = points;
            result.TimeSpent = timeSpent;

            repository.Update(result);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteResultAsync(string id)
        {
            var result = await repository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            repository.Delete(result);
            await repository.SaveChangesAsync();
        }
    }
}
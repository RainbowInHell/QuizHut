namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;
    
    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class ResultsService : IResultsService
    {
        private readonly IRepository<Result> repository;

        private readonly IRepository<Event> eventRepository;

        private readonly IExpressionBuilder expressionBuilder;

        public ResultsService(
            IRepository<Result> repository,
            IRepository<Event> eventRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.repository = repository;
            this.eventRepository = eventRepository;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<int> GetResultsCountByStudentIdAsync(string id, string searchCriteria = null, string searchText = null)
        {
            var query = repository
                .AllAsNoTracking()
                //.Where(x => x.StudentId == id && x.Event.Quizzes.Any(x => x.Id == ""))
                .Where(x => x.StudentId == id);

            if (searchCriteria != null && searchText != null)
            {
                var filter = expressionBuilder.GetExpression<Result>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<string> CreateResultAsync(string studentId, decimal points, string quizId)
        {
            var @event = await eventRepository
                .All()
                .Include(e => e.Quizzes)
                .FirstOrDefaultAsync(e => e.Quizzes.Any(q => q.Id == quizId));

            var quiz = @event.Quizzes.FirstOrDefault(q => q.Id == quizId);

            var result = new Result()
            {
                Points = points,
                StudentId = studentId,
                MaxPoints = quiz.Questions.Count,
                EventId = @event.Id,
                EventName = @event.Name,
                QuizName = quiz.Name,
                EventActivationDateAndTime = @event.ActivationDateAndTime,
            };

            await repository.AddAsync(result);
            await repository.SaveChangesAsync();

            @event.Results.Add(result);
            await eventRepository.SaveChangesAsync();

            return result.Id;
        }

        public async Task UpdateResultAsync(string id, decimal points)
        {
            var result = await repository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            result.Points = points;

            repository.Update(result);
            await repository.SaveChangesAsync();
        }
    }
}
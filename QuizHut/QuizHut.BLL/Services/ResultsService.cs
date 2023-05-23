using Microsoft.EntityFrameworkCore;
using QuizHut.BLL.Expression.Contracts;
using QuizHut.BLL.Services.Contracts;
using QuizHut.DLL.Entities;
using QuizHut.DLL.Repositories.Contracts;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizHut.BLL.Services
{
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
        public async Task<string> CreateResultAsync(string studentId, int points, int maxPoints, string quizId)
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
                MaxPoints = maxPoints,
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
    }
}
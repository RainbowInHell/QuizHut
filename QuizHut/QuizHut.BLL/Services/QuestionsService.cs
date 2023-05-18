namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class QuestionsService : IQuestionsService
    {
        private readonly IRepository<Question> repository;

        private readonly IRepository<Quiz> quizRepository;

        public QuestionsService(IRepository<Question> repository, IRepository<Quiz> quizRepository)
        {
            this.repository = repository;
            this.quizRepository = quizRepository;
        }

        public async Task<IList<T>> GetAllByQuizIdAsync<T>(string id)
        {
            return await repository.AllAsNoTracking()
                .Where(x => x.QuizId == id)
                .OrderBy(x => x.Number)
                .To<T>()
                .ToListAsync();
        }

        public async Task<string> CreateQuestionAsync(string quizId, string questionText)
        {
            var quiz = await quizRepository.AllAsNoTracking().Select(x => new
            {
                x.Id,
                Questions = x.Questions.Count(),
            }).FirstOrDefaultAsync(x => x.Id == quizId);

            var question = new Question
            {
                Number = quiz.Questions + 1,
                Text = questionText,
                QuizId = quizId,
            };

            await repository.AddAsync(question);
            await repository.SaveChangesAsync();

            return question.Id;
        }

        public async Task Update(string id, string text)
        {
            var question = await repository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            question.Text = text;
            repository.Update(question);
            
            await repository.SaveChangesAsync();
        }

        public async Task UpdateAllQuestionsInQuizNumeration(string quizId)
        {
            var count = 0;

            var questions = repository
              .AllAsNoTracking()
              .Where(x => x.QuizId == quizId)
              .OrderBy(x => x.Number);

            foreach (var question in questions)
            {
                question.Number = ++count;
                repository.Update(question);
            }

            await repository.SaveChangesAsync();
        }

        public async Task DeleteQuestionByIdAsync(string id)
        {
            var question = await repository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            repository.Delete(question);
            await repository.SaveChangesAsync();
            
            await this.UpdateAllQuestionsInQuizNumeration(question.QuizId);
        }
    }
}
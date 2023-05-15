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
        private readonly IDeletableEntityRepository<Quiz> quizRepository;

        private readonly IRepository<Password> passwordRepository;

        private readonly IExpressionBuilder expressionBuilder;

        public QuizzesService(
            IDeletableEntityRepository<Quiz> quizRepository,
            IRepository<Password> passwordRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.quizRepository = quizRepository;
            this.passwordRepository = passwordRepository;
            this.expressionBuilder = expressionBuilder;
        }

        // public async Task<string> CreateQuizAsync(string name, string description, int? timer, string creatorId, string password)
        // {
        //     var quiz = new Quiz
        //     {
        //         Name = name,
        //         Description = description,
        //         Timer = timer,
        //         CreatorId = creatorId,
        //     };

        //     var passwordEntitiy = new Password() { Content = password, QuizId = quiz.Id };
        //     await passwordRepository.AddAsync(passwordEntitiy);
        //     await passwordRepository.SaveChangesAsync();

        //     quiz.PasswordId = passwordEntitiy.Id;
        //     await quizRepository.AddAsync(quiz);
        //     await quizRepository.SaveChangesAsync();

        //     return quiz.Id;
        // }

        // public async Task<IList<T>> GetAllUnAssignedToEventAsync<T>(string creatorId = null)
        // {
        //     var query = quizRepository
        //            .AllAsNoTracking()
        //            .Where(x => x.EventId == null);

        //     if (creatorId != null)
        //     {
        //         query = query.Where(x => x.CreatorId == creatorId);
        //     }

        //     return await query.OrderByDescending(x => x.CreatedOn)
        //         .To<T>()
        //         .ToListAsync();
        // }

        // public async Task<T> GetQuizByIdAsync<T>(string id)
        //=> await quizRepository
        //        .AllAsNoTracking()
        //        .Where(x => x.Id == id)
        //        .To<T>()
        //        .FirstOrDefaultAsync();

        // public async Task DeleteByIdAsync(string id)
        // {
        //     var quiz = await quizRepository
        //         .AllAsNoTracking()
        //         .FirstOrDefaultAsync(x => x.Id == id);
        //     var password = await passwordRepository
        //         .AllAsNoTracking()
        //         .Where(x => x.QuizId == id)
        //         .FirstOrDefaultAsync();
        //     passwordRepository.Delete(password);
        //     quizRepository.Delete(quiz);

        //     await passwordRepository.SaveChangesAsync();
        //     await quizRepository.SaveChangesAsync();
        // }

        // public async Task UpdateAsync(string id, string name, string description, int? timer, string password)
        // {
        //     var quiz = await quizRepository
        //        .AllAsNoTracking()
        //        .FirstOrDefaultAsync(x => x.Id == id);
        //     var passwordEntity = await passwordRepository
        //         .AllAsNoTracking()
        //         .Where(x => x.QuizId == id)
        //         .FirstOrDefaultAsync();

        //     if (passwordEntity.Content != password)
        //     {
        //         passwordEntity.Content = password;
        //         passwordRepository.Update(passwordEntity);
        //         await passwordRepository.SaveChangesAsync();
        //     }

        //     quiz.Name = name;
        //     quiz.Description = description;
        //     quiz.Timer = timer;

        //     quizRepository.Update(quiz);
        //     await quizRepository.SaveChangesAsync();
        // }

        // public async Task<string> GetQuizIdByPasswordAsync(string password)
        // => await quizRepository.AllAsNoTracking()
        //     .Where(x => x.Password.Content == password)
        //     .Select(x => x.Id)
        //     .FirstOrDefaultAsync();

        // public async Task<bool[]> HasUserPermition(string userId, string quizId, bool isQuizTaken)
        // {
        //     var quizQuery = quizRepository
        //         .AllAsNoTracking()
        //         .Where(x => x.Id == quizId);

        //     var creatorId = await quizQuery.Select(x => x.CreatorId).FirstOrDefaultAsync();
        //     if (creatorId == userId)
        //     {
        //         return new bool[] { true, true };
        //     }

        //     if (isQuizTaken)
        //     {
        //         return new bool[] { false, false };
        //     }

        //     var eventIsActive = await quizQuery.Select(x => x.Event.Status == Status.Active).FirstOrDefaultAsync();
        //     if (!eventIsActive)
        //     {
        //         return new bool[] { false, false };
        //     }

        //     var results = await quizQuery
        //         .SelectMany(x => x.Event.Results.Where(x => x.StudentId == userId))
        //         .ToListAsync();
        //     if (results.Count() > 0)
        //     {
        //         return new bool[] { false, false };
        //     }

        //     var eventGroups = await quizQuery
        //         .SelectMany(x => x.Event.EventsGroups
        //         .Where(x => x.Group.StudentstGroups
        //         .Any(x => x.StudentId == userId)))
        //         .ToListAsync();

        //     return eventGroups.Count() > 0 ? new bool[] { true, false } : new bool[] { false, false };
        // }

        // public async Task AssignQuizToEventAsync(string eventId, string quizId)
        // {
        //     var quiz = await quizRepository
        //         .AllAsNoTracking()
        //         .Where(x => x.Id == quizId)
        //         .FirstOrDefaultAsync();

        //     quiz.EventId = eventId;
        //     quizRepository.Update(quiz);
        //     await quizRepository.SaveChangesAsync();
        // }

        // public async Task DeleteEventFromQuizAsync(string eventId, string quizId)
        // {
        //     var quiz = await quizRepository
        //         .AllAsNoTracking()
        //         .Where(x => x.Id == quizId)
        //         .FirstOrDefaultAsync();

        //     quiz.EventId = null;
        //     quizRepository.Update(quiz);
        //     await quizRepository.SaveChangesAsync();
        // }

        public async Task<IList<T>> GetUnAssignedToCategoryByCreatorIdAsync<T>(string categoryId, string creatorId)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CreatorId == creatorId && x.CategoryId != categoryId)
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

        //public async Task<string> GetCreatorIdByQuizIdAsync(string id)
        //=> await quizRepository
        //        .AllAsNoTracking()
        //        .Where(x => x.Id == id)
        //        .Select(x => x.CreatorId)
        //        .FirstOrDefaultAsync();

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

        //public async Task<string> GetQuizNameByIdAsync(string id)
        //=> await quizRepository.AllAsNoTracking()
        //     .Where(x => x.Id == id)
        //     .Select(x => x.Name)
        //     .FirstOrDefaultAsync();
    }
}
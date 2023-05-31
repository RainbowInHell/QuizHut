namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;

    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    //Класс для работы с викторинами
    public class QuizzesService : IQuizzesService
    {
        //Репозиторий типа викторины
        private readonly IRepository<Quiz> quizRepository;

        private readonly IExpressionBuilder expressionBuilder;

        //Конструктор класса
        public QuizzesService(
            IRepository<Quiz> quizRepository,
            IExpressionBuilder expressionBuilder)
        {
            this.quizRepository = quizRepository;
            this.expressionBuilder = expressionBuilder;
        }

        //Метод получения всех викторин по Id создателя, Id категории и критериям поиска
        public async Task<IEnumerable<T>> GetAllQuizzesAsync<T>(
            string creatorId = null,
            string searchCriteria = null,
            string searchText = null,
            string categoryId = null)
        {
            //Получаем все викторины
            var query = quizRepository.AllAsNoTracking();

            //Если Id создателя указано, то добавить в выборку
            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            //Если Id категории указано, то добавить в выборку
            if (categoryId != null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            //Если текст для поиска указан, то добавить в выборку
            var emptyNameInput = searchText == null && searchCriteria == "Name";
            if (searchCriteria != null && !emptyNameInput)
            {
                var filter = expressionBuilder.GetExpression<Quiz>(searchCriteria, searchText);
                query = query.Where(filter);
            }

            //Отсортировать по убыванию и вернуть
            return await query.OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }

        //Метод получения всех викторин по Id категории
        public async Task<IList<T>> GetQuizzesByCategoryIdAsync<T>(string id)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == id)
                .To<T>()
                .ToListAsync();
        }

        //Метод получения всех викторин, которые не назначены на категорию с переданным Id
        public async Task<IList<T>> GetUnAssignedQuizzesToCategoryByCreatorIdAsync<T>(string creatorId)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.CreatorId == creatorId && x.CategoryId == null)
                .To<T>()
                .ToListAsync();
        }

        //Метод получения всех викторин, которые не назначены на событие
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

        //Метод получения всех викторин, которые назначены на событие с переданным Id
        public async Task<IList<T>> GetQuizzesByEventId<T>(string eventId)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.EventId == eventId)
                .To<T>()
                .ToListAsync();
        }

        //Метод получения викторины с переданным паролем
        public async Task<T> GetQuizByPasswordAsync<T>(string password)
        {
            return await quizRepository
                .AllAsNoTracking()
                .Where(x => x.Password == password)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        //Метод создания викторины
        public async Task<string> CreateQuizAsync(string name, string description, int? timer, string creatorId, string password)
        {
            //Создание нового объекта викторины
            var quiz = new Quiz
            {
                Name = name,
                Description = description,
                Timer = timer,
                CreatorId = creatorId,
                Password = password
            };

            //Добавление новой викторины
            await quizRepository.AddAsync(quiz);
            //Сохранение изменений в базе данных
            await quizRepository.SaveChangesAsync();

            return quiz.Id;
        }

        //Метод обновления викторины
        public async Task UpdateQuizAsync(string id, string name, string description, int? timer, string password)
        {
            var quiz = await quizRepository
               .All()
               .FirstOrDefaultAsync(x => x.Id == id);

            //Установка новых значений для полей
            quiz.Name = name;
            quiz.Description = description;
            quiz.Timer = timer;
            quiz.Password = password;

            //Обновление викторины
            quizRepository.Update(quiz);
            //Сохранение изменений в базе данных
            await quizRepository.SaveChangesAsync();
        }

        //Метод удаления викторины
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
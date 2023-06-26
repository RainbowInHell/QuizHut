namespace QuizHut.DLL.Seeding
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DLL.Entities;
    using QuizHut.DLL.EntityFramework;
    using QuizHut.DLL.Seeding.Contracts;

    public class DataSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var organizerId = await CreateUser(userManager, "Пример", "Организатора", "organizer@mail.ru", "Organizer");

            if (organizerId != null)
            {
                await CreateUser(userManager, "Матвей", "Быковский", "bykovsky@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Анастасия", "Бразинская", "braniskaya@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Владислав", "Вишневскй", "vishnevski@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Даниил", "Григоренко", "grigorenko@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Алексей", "Волчек", "volcheck@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Алексей", "Заяц", "zayac@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Кирилл", "Карпекин", "karpekin@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Илья", "Миклашевич", "miklashevich@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Денис", "Раткевич", "ratkevich@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Владимир", "Климченя", "klimchenya@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Павел", "Золотарев", "zolotarev@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Мария", "Комкова", "komkova@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Карина", "Липская", "lipskaya@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Артем", "Зеленок", "zelenok@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Егор", "Чужавко", "chyzhavko@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Александр", "Янушевич", "yanushevich@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Алексей", "Курышев", "kyrishev@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Дарина", "Сулим", "sulim@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Давид", "Орлис", "bykovsky@mail.ru", "Student", organizerId);
                await CreateUser(userManager, "Даниил", "Бяроско", "bykovsky@mail.ru", "Student", organizerId);

                await CreateQuizzesAsync(organizerId, userManager, dbContext);

                await CreateCategoryAsync(organizerId, "Алгоритмы", dbContext);
                await CreateCategoryAsync(organizerId, "Математика", dbContext);
                await CreateCategoryAsync(organizerId, "ООП", dbContext);
                await CreateCategoryAsync(organizerId, "БазыДанных", dbContext);
                await CreateCategoryAsync(organizerId, "Гит", dbContext);

                await CreateGroupAsync(organizerId, "T-991", dbContext);
                await CreateGroupAsync(organizerId, "T-992", dbContext);
                await CreateGroupAsync(organizerId, "T-993", dbContext);
                await CreateGroupAsync(organizerId, "T-994", dbContext);
                await CreateGroupAsync(organizerId, "T-995", dbContext);
            }
        }

        #region QuizzesCreation

        private static async Task CreateQuizzesAsync(string organizerId, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            var teacher = await userManager.FindByIdAsync(organizerId);
            for (int i = 1; i <= 5; i++)
            {
                var quiz = new Quiz()
                {
                    Name = $"Тестовая викторина {i}",
                    Password = $"password{i}",
                    Description = $"<p>Это тестовая викторина{i}</p>",
                };

                await dbContext.Quizzes.AddAsync(quiz);

                for (int j = 1; j <= 10; j++)
                {
                    var question = new Question()
                    {
                        Text = $"<p>Вопрос {j}</p>",
                        Number = j,
                    };

                    for (int l = 1; l <= 3; l++)
                    {
                        var answer = new Answer
                        {
                            Text = l % 2 == 0 ? "<p>Верный ответ</p>" : "<p>Неверный ответ</p>",
                            IsRightAnswer = l % 2 == 0 ? true : false,
                        };

                        await dbContext.Answers.AddAsync(answer);
                        question.Answers.Add(answer);
                    }

                    await dbContext.Questions.AddAsync(question);
                    quiz.Questions.Add(question);
                }

                teacher.CreatedQuizzes.Add(quiz);

                await userManager.UpdateAsync(teacher);
                await dbContext.SaveChangesAsync();
            }
        }

        #endregion

        #region UserCreation

        private static async Task<string> CreateUser(
            UserManager<ApplicationUser> userManager,
            string firstName,
            string lastName,
            string email,
            string roleName,
            string teacherId = null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                TeacherId = teacherId
            };

            var result = await userManager.CreateAsync(user, "123456");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
                return user.Id;
            }

            return null;
        }

        #endregion

        #region CategoryCreation

        private static async Task CreateCategoryAsync(string organizerId, string categoryName, ApplicationDbContext dbContext)
        {
            var category = new Category
            {
                CreatorId = organizerId,
                Name = categoryName
            };

            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
        }

        #endregion

        #region GroupCreation

        private static async Task CreateGroupAsync(string organizerId, string groupName, ApplicationDbContext dbContext)
        {
            var group = new Group
            {
                CreatorId = organizerId,
                Name = groupName
            };

            dbContext.Groups.Add(group);
            await dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
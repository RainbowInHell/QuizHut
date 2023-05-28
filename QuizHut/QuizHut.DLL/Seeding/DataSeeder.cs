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

            var teacherId = await CreateUser(
                userManager,
                "TestTeacher",
                "organizer@mail.ru",
                "Organizer");

            await CreateUser(
                userManager,
                "TestStudent",
                "student@mail.ru",
                "Student");

            if (!dbContext.Quizzes.Any())
            {
                await CreateQuizzes(teacherId, userManager, dbContext);
            }
        }

        private static async Task CreateQuizzes(string teacherId, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            var teacher = await userManager.FindByIdAsync(teacherId);
            for (int i = 1; i <= 5; i++)
            {
                var quiz = new Quiz()
                {
                    Name = $"Test quiz {i}",
                    Password = $"password{i}",
                    Description = $"<p>This is test quiz {i}</p>",
                };

                await dbContext.Quizzes.AddAsync(quiz);

                for (int j = 1; j <= 10; j++)
                {
                    var question = new Question()
                    {
                        Text = $"<p>Question {j}</p>",
                        Number = j,
                    };

                    for (int l = 1; l <= 3; l++)
                    {
                        var answer = new Answer
                        {
                            Text = l % 2 == 0 ? "<p>True answer</p>" : "<p>False answer</p>",
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

        private static async Task<string> CreateUser(
            UserManager<ApplicationUser> userManager,
            string name,
            string email,
            string roleName)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                FirstName = name,
                LastName = name,
                Email = email,
            };

            var result = await userManager.CreateAsync(user, "123456");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }

            return user.Id;
        }
    }
}
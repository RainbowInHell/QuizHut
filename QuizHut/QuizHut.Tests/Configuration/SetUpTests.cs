namespace QuizHut.Tests.Configuration
{
    using Hangfire;
    using Hangfire.MemoryStorage;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.BLL.Expression;
    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.EntityFramework;
    using QuizHut.DLL.Repositories;
    using QuizHut.DLL.Repositories.Contracts;
    using SendGrid.Extensions.DependencyInjection;

    public class SetUpTests
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ApplicationDbContext DbContext { get; set; }

        public SetUpTests()
        {
            var services = SetServices();

            ServiceProvider = services.BuildServiceProvider();
            DbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            AutoMapperConfig.RegisterMappings(typeof(App).Assembly);

            services.AddSendGrid(opt => opt.ApiKey = "test");

            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IExpressionBuilder, ExpressionBuilder>();
            services.AddTransient<IQuizzesService, QuizzesService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<IEventsGroupsService, EventsGroupsService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<IResultsService, ResultsService>();
            services.AddTransient<IEventsService, EventsService>();
            //services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IStudentsGroupsService, StudentsGroupsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IScheduledJobsService, ScheduledJobsService>();

            return services;
        }
    }
}
namespace QuizHut.Infrastructure.Registrars
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Expression;
    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Services;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.Services;

    using SendGrid.Extensions.DependencyInjection;

    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //setx QH_SENDGRID_APIKEY "API_KEY_EXAMPLE"
            const string QH_SENDGRID_APIKEY = "QH_SENDGRID_APIKEY";

            //setx QH_SENDGRID_FROM_EMAIL "quizhutkbip@gmail.com"
            const string QH_SENDGRID_FROM_EMAIL = "QH_SENDGRID_FROM_EMAIL";

            var apiKey = Environment.GetEnvironmentVariable("QH_SENDGRID_APIKEY", EnvironmentVariableTarget.User);
            
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException($"The {QH_SENDGRID_APIKEY} environment variable is not set.");
            }
            
            var fromEmail = Environment.GetEnvironmentVariable("QH_SENDGRID_FROM_EMAIL", EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(fromEmail))
            {
                throw new InvalidOperationException($"The {QH_SENDGRID_FROM_EMAIL} environment variable is not set.");
            }

            services.AddSendGrid(opt => opt.ApiKey = apiKey);

            services.AddSingleton<LoginRequestValidator>();
            services.AddSingleton<RegisterRequestValidator>();
            services.AddSingleton<EmailRequestValidator>();
            services.AddSingleton<PasswordRequestValidator>();

            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IStudentsGroupsService, StudentsGroupsService>();
            services.AddTransient<IEventsGroupsService, EventsGroupsService>();
            services.AddTransient<IQuizzesService, QuizzesService>();
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IScheduledJobsService, ScheduledJobsService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<IResultsService, ResultsService>();
            services.AddTransient<IDateTimeConverter, DateTimeConverter>();
            services.AddTransient<IExpressionBuilder, ExpressionBuilder>();
            services.AddTransient<IShuffler, Shuffler>();
            services.AddTransient<IResultHelper, ResultHelper>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserDialogService, UserDialogService>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IViewDisplayTypeService, ViewDisplayTypeService>();
            services.AddSingleton<ISharedDataStore, SharedDataStore>();

            return services;
        }
    }
}
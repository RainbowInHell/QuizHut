namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSendGrid(opt =>
                opt.ApiKey = configuration.GetValue<string>("EmailSender:ApiKey"));

            services.AddSingleton<LoginRequestValidator>();
            services.AddSingleton<RegisterRequestValidator>();
            services.AddSingleton<EmailRequestValidator>();
            services.AddSingleton<PasswordRequestValidator>();

            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<IStudentsGroupsService, StudentsGroupsService>();
            services.AddTransient<IEventsGroupsService, EventsGroupsService>();
            services.AddTransient<IExpressionBuilder, ExpressionBuilder>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserDialogService, UserDialogService>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IGroupSettingsTypeService, GroupSettingsTypeService>();
            services.AddSingleton<ISharedDataStore, SharedDataStore>();

            return services;
        }
    }
}
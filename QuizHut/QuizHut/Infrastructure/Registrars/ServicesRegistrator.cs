namespace QuizHut.Infrastructure.Registrars
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.BLL.Dto;
    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Services;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.Services;
    using QuizHut.ViewModels.Base;

    using SendGrid.Extensions.DependencyInjection;

    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSendGrid(opt =>
                opt.ApiKey = configuration.GetValue<string>("EmailSender:ApiKey"));

            services.AddSingleton<EmailRequest>();
            services.AddSingleton<PasswordRequest>();

            services.AddSingleton<LoginRequestValidator>();
            services.AddSingleton<RegisterRequestValidator>();
            services.AddSingleton<EmailRequestValidator>();
            services.AddSingleton<PasswordRequestValidator>();

            services.AddSingleton<Func<Type, ViewModel>>(services => viewModelType => (ViewModel)services.GetRequiredService(viewModelType));

            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserDialog, UserDialogService>();

            services.AddTransient<IUserAccountService, UserAccountService>();

            return services;
        }
    }
}
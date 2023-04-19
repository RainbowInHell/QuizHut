namespace QuizHut.Infrastructure.Registrars
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            services.AddSendGrid(options => options.ApiKey = configuration.GetValue<string>(EmailSenderService.ApiKey));

            services.AddSingleton<IStringEncoderDecoder, StringEncoderDecoder>();
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(services => viewModelType => (ViewModel)services.GetRequiredService(viewModelType));

            return services;
        }
    }
}
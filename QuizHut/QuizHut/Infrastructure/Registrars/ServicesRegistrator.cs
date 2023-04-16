namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Services;

    using SendGrid.Extensions.DependencyInjection;

    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSendGrid(options => options.ApiKey = configuration.GetValue<string>(EmailSenderService.ApiKey));

            services.AddSingleton<IStringEncoderDecoder, StringEncoderDecoder>();
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IUserAccountService, UserAccountService>();

            return services;
        }
    }
}
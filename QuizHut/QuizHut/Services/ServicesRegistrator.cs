namespace QuizHut.Services
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.ViewModels;

    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<LoginViewModel>()
            ;
    }
}
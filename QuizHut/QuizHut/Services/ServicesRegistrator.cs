namespace QuizHut.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using QuizHut.DAL.Entities;
    using QuizHut.Services.Contracts;
    using QuizHut.ViewModels;

    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IAuthService, AuthService>()
            ;
    }
}
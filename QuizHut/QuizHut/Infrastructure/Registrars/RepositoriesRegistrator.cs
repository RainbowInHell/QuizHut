namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DLL.Repositories;
    using QuizHut.DLL.Repositories.Contracts;

    public static class RepositoriesRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return services;
        }
    }
}
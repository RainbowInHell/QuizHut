namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DAL.Common;
    using QuizHut.DAL.Common.Repositories;
    using QuizHut.DAL.EntityFramework;
    using QuizHut.DAL.Repositories;

    public static class RepositoriesRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            return services;
        }
    }
}
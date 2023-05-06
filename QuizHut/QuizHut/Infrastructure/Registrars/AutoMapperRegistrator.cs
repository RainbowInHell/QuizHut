namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.DependencyInjection;
    using QuizHut.BLL.MapperConfig;

    public static class AutoMapperRegistrator
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfig));

            return services;
        }
    }
}
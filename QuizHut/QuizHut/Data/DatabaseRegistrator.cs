namespace QuizHut.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DAL.EntityFramework;

    public static class DatabaseRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) => services
            .AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseMySQL(configuration.GetConnectionString("QuizHutDatabase"));
            });
    }
}
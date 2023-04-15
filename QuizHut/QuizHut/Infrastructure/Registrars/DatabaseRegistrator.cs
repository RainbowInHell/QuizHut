namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DAL.Entities;
    using QuizHut.DAL.EntityFramework;

    public static class DatabaseRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                var databaseType = configuration["Type"];

                opt.UseMySQL(configuration.GetConnectionString(databaseType));
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            });


            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
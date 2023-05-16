namespace QuizHut.Infrastructure.Registrars
{
    using System;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DLL.Entities;
    using QuizHut.DLL.EntityFramework;

    public static class DatabaseRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                //setx QH_DATABASE_CONNECTION_STRING "Server=localhost;Database=QuizHut;Uid=root;Pwd=matvey2003;"
                const string QH_DATABASE_CONNECTION_STRING = "QH_DATABASE_CONNECTION_STRING";

                var connectionString = Environment.GetEnvironmentVariable(QH_DATABASE_CONNECTION_STRING, EnvironmentVariableTarget.User);

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException($"The {QH_DATABASE_CONNECTION_STRING} environment variable is not set.");
                }

                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                opt.EnableSensitiveDataLogging();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddRepositories();

            return services;
        }
    }
}
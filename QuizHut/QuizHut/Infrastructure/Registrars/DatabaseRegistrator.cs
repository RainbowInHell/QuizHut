namespace QuizHut.Infrastructure.Registrars
{
    using System;

    using Hangfire;
    using Hangfire.MySql;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.DLL.Entities;
    using QuizHut.DLL.EntityFramework;

    public static class DatabaseRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            const string QH_DATABASE_CONNECTION_STRING = "QH_DATABASE_CONNECTION_STRING";

            var connectionString = Environment.GetEnvironmentVariable(QH_DATABASE_CONNECTION_STRING, EnvironmentVariableTarget.User);

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException($"The {QH_DATABASE_CONNECTION_STRING} environment variable is not set.");
                }

                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                opt.EnableSensitiveDataLogging();
            });

            services.AddHangfire(configruation =>
            {
                configruation.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(3),
                        JobExpirationCheckInterval = TimeSpan.FromSeconds(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire",
                    })).WithJobExpirationTimeout(TimeSpan.FromHours(24 * 7));
            }).AddHangfireServer(option =>
            {
                option.SchedulePollingInterval = TimeSpan.FromSeconds(1);
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

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            return services;
        }
    }
}
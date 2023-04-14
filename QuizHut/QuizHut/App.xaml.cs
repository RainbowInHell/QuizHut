namespace QuizHut
{
    using System;
    using System.Windows;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using QuizHut.DAL.Entities;
    using QuizHut.DAL.EntityFramework;
    using QuizHut.Data;
    using QuizHut.Services;
    using QuizHut.Services.Contracts;
    using QuizHut.ViewModels;

    public partial class App : Application
    {
        private static IHost host;

        public static IHost Host => host
            ??= Program.CreareHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration.GetSection("Database"))
            .AddScoped<SignInManager<ApplicationUser>>()
            .AddScoped<UserManager<ApplicationUser>>()
            .AddScoped<IAuthService, AuthService>()
            .AddServices()
            .AddViewModels()
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            base.OnStartup(e);

            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (var host = Host)
            {
                base.OnExit(e);

                await host.StopAsync();
            }
        }
    }
}
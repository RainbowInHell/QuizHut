namespace QuizHut
{
    using System;
    using System.Windows;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using QuizHut.Infrastructure.Registrars;

    public partial class App : Application
    {
        private static IHost? host;

        public static IHost Host => host
            ??= Program.CreareHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration.GetSection("Database"))
            .AddMapper()
            .AddRepositories()
            .AddServices(host.Configuration)
            .AddViewModels()
            ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await Host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (Host)
            {
                base.OnExit(e);

                await Host.StopAsync();
            }
        }
    }
}
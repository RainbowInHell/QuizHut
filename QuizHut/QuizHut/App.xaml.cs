namespace QuizHut
{
    using System;
    using System.Windows;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using QuizHut.Data;
    using QuizHut.Services;
    using QuizHut.ViewModels;

    public partial class App : Application
    {
        private static IHost host;

        public static IHost Host => host
            ??= Program.CreareHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration.GetSection("Database"))
            .AddServices()
            .AddViewModels()
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
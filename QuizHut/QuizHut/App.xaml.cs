namespace QuizHut
{
    using System;
    using System.Windows;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using QuizHut.BLL.MapperConfig;
    using QuizHut.Infrastructure.Registrars;
    using QuizHut.Infrastructure.Services.Contracts;

    public partial class App : Application
    {
        private static IHost? host;

        public static IHost Host => host
            ??= Program.CreareHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase()
            .AddRepositories()
            .AddServices()
            .AddViewModels()
            .AddViews();

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host.StartAsync();

            AutoMapperConfig.RegisterMappings(typeof(App).Assembly);

            Services.GetRequiredService<IUserDialogService>().OpenMainView();

            base.OnStartup(e);
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
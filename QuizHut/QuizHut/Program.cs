namespace QuizHut
{
    using System;

    using Microsoft.Extensions.Hosting;

    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var app = new App();

            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreareHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(App.ConfigureServices);
    }
}
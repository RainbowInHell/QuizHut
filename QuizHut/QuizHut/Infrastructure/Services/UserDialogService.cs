namespace QuizHut.Infrastructure.Services
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.Views.Windows;
    
    class UserDialogService : IUserDialog
    {
        private readonly IServiceProvider Services;

        public UserDialogService(IServiceProvider services) => Services = services;

        private LoginView? LoginView;
        public void OpenLoginView()
        {
            if(LoginView is { } window)
            {
                window.Show();
                return;
            }

            window = Services.GetRequiredService<LoginView>();
            window.Closed += (_, _) => LoginView = null;

            LoginView = window;
            window.Show();

        }

        private MainView? MainView;
        public void OpenMainView()
        {
            if (MainView is { } window)
            {
                window.Show();
                return;
            }

            window = Services.GetRequiredService<MainView>();
            window.Closed += (_, _) => MainView = null;

            MainView = window;
            window.Show();
        }
    }
}
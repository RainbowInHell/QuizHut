namespace QuizHut.Infrastructure.Services
{
    using System;

    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class NavigationService : INavigationService
    {
        private ViewModel currentView;
        public ViewModel CurrentView 
        {
            get => currentView;
            set
            {
                currentView = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
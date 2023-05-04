namespace QuizHut.Infrastructure.Commands
{
    using System;
    using System.Windows.Input;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Factory;

    class NavigationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigationService navigationService;
        private readonly ISimpleTraderViewModelFactory viewModelFactory;

        public NavigationCommand(INavigationService navigationService, ISimpleTraderViewModelFactory viewModelFactory)
        {
            this.navigationService = navigationService;
            this.viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                navigationService.CurrentView = viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}

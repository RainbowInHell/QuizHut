namespace QuizHut.Infrastructure.Commands
{
    using System;

    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Factory;

    class NavigationCommand : Command
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigationService navigationService;
        private readonly IViewModelFactory viewModelFactory;

        public NavigationCommand(INavigationService navigationService, IViewModelFactory viewModelFactory)
        {
            this.navigationService = navigationService;
            this.viewModelFactory = viewModelFactory;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                ViewModel viewModel = viewModelFactory.CreateViewModel(viewType);
                navigationService.CurrentView = viewModel;
            }
        }
    }
}
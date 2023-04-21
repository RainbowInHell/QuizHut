namespace QuizHut.Infrastructure.Commands
{
    using System;

    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Services.Contracts;

    class NavigationCommand : Command
    {
        private readonly Type viewModelType;
        private readonly INavigationService navigationService;

        public NavigationCommand(Type viewModelType, INavigationService navigationService)
        {
            this.viewModelType = viewModelType;
            this.navigationService = navigationService;
        }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            navigationService.NavigateTo(viewModelType);
        }
    }
}

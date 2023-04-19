namespace QuizHut.ViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class StudentRegistrationViewModel : ViewModel
    {
        private INavigationService navigationService;

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        public StudentRegistrationViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            NavigateAuthorizationViewCommand = new ActionCommand(OnNavigateAuthorizationViewCommandExecuted, CanNavigateAuthorizationViewCommandExecute);
        }

        #region NavigateAuthorizationViewCommand

        public ICommand NavigateAuthorizationViewCommand { get; }

        private void OnNavigateAuthorizationViewCommandExecuted(object p) { NavigationService.NavigateTo<AuthorizationViewModel>(); }

        private bool CanNavigateAuthorizationViewCommandExecute(object p) => true;

        #endregion
    }
}
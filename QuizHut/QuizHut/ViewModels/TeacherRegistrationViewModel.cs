namespace QuizHut.ViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class TeacherRegistrationViewModel : ViewModel
    {
        private INavigationService navigationService;

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        public TeacherRegistrationViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            NavigateAuthorizationViewCommand = new ActionCommand(OnNavigateAuthorizationViewCommandExecuted, CanNavigateAuthorizationViewCommandExecute);
            NavigateStudentRegistrationViewCommand = new ActionCommand(OnNavigateStudentRegistrationViewCommandExecuted, CanNavigateStudentRegistrationViewCommandExecute);
        }

        #region NavigateAuthorizationViewCommand

        public ICommand NavigateAuthorizationViewCommand { get; }

        private void OnNavigateAuthorizationViewCommandExecuted(object p) { NavigationService.NavigateTo<AuthorizationViewModel>(); }

        private bool CanNavigateAuthorizationViewCommandExecute(object p) => true;

        #endregion

        #region NavigateStudentRegistrationViewCommand

        public ICommand NavigateStudentRegistrationViewCommand { get; }

        private void OnNavigateStudentRegistrationViewCommandExecuted(object p) { NavigationService.NavigateTo<StudentRegistrationViewModel>(); }

        private bool CanNavigateStudentRegistrationViewCommandExecute(object p) => true;

        #endregion
    }
}
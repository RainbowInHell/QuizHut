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

            NavigateAuthorizationViewCommand = new NavigationCommand(typeof(AuthorizationViewModel), navigationService);
            NavigateStudentRegistrationViewCommand = new NavigationCommand(typeof(StudentRegistrationViewModel), navigationService);
        }

        #region NavigateAuthorizationViewCommand

        public ICommand NavigateAuthorizationViewCommand { get; }

        #endregion

        #region NavigateStudentRegistrationViewCommand

        public ICommand NavigateStudentRegistrationViewCommand { get; }

        #endregion
    }
}
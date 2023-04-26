namespace QuizHut.ViewModels.LoginViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class TeacherRegistrationViewModel : ViewModel
    {
        public TeacherRegistrationViewModel(IRenavigator authorizRenavigator, IRenavigator studentRegistrRenavigator)
        {
            NavigateAuthorizationViewCommand = new RenavigateCommand(authorizRenavigator);
            NavigateStudentRegistrationViewCommand = new RenavigateCommand(studentRegistrRenavigator);
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

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
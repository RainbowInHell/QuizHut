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

        #region Commands

        public ICommand NavigateAuthorizationViewCommand { get; }

        public ICommand NavigateStudentRegistrationViewCommand { get; }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
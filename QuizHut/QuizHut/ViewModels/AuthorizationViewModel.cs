namespace QuizHut.ViewModels
{
    using QuizHut.Infrastructure.Commands;
    using QuizHut.ViewModels.Base;

    using System.Security;
    using System.Windows;
    using System.Windows.Input;

    internal class AuthorizationViewModel : ViewModel
    {
        //Fields
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        #region Commands

        public ICommand LoginCommand { get; }
        private void OnLoginCommandExecuted(object p)
        {
            MessageBox.Show("Username: " + Username + "\nPassword: " + Password.ToString());
        }
        private bool CanLoginCommandExecute(object p)
        {
            bool result = true;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 5)
                result = false;
            return result;
        }

        #endregion

        public AuthorizationViewModel()
        {
            #region Commands

            LoginCommand = new ActionCommand(OnLoginCommandExecuted, CanLoginCommandExecute);

            #endregion
        }
    }
}

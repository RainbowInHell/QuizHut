namespace QuizHut.ViewModels
{
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Services;
    using QuizHut.Services.Contracts;
    using QuizHut.ViewModels.Base;

    using System.Security;
    using System.Windows;
    using System.Windows.Input;

    internal class AuthorizationViewModel : ViewModel
    {
        private readonly IAuthService authService;

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
        private async void OnLoginCommandExecuted(object p)
        {
            bool loginSuccessful = await authService.LoginAsync(Username, Password);

            if (loginSuccessful)
            {
                MessageBox.Show("Good!");
            }
            else
            {
                MessageBox.Show("Bad!");
            }

            //MessageBox.Show("Username: " + Username + "\nPassword: " + Password.ToString());
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

        public AuthorizationViewModel(IAuthService authService)
        {
            this.authService = authService;
            #region Commands

            LoginCommand = new ActionCommand(OnLoginCommandExecuted, CanLoginCommandExecute);

            #endregion
        }
    }
}

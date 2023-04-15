namespace QuizHut.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;

    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class AuthorizationViewModel : ViewModel
    {
        private readonly IAuthService authService;

        public AuthorizationViewModel(IAuthService authService)
        {
            this.authService = authService;

            LoginCommandAsync = new ActionCommandAsync(OnLoginCommandExecuted, CanLoginCommandExecute);
        }

        private string? email;

        private string? password;

        public string? Email
        {
            get => email;
            set => Set(ref email, value);
        }

        public string? Password
        {
            get => password;
            set => Set(ref password, value);
        }

        #region LoginCommand

        public IAsyncCommand LoginCommandAsync { get; }

        private async Task OnLoginCommandExecuted(object p)
        {
            //bool loginSuccessful = await authService.LoginAsync(UserName, Password);
            bool loginSuccessful = await authService.ResetPasswordAsync(Email);

            if (loginSuccessful)
            {
                MessageBox.Show("Good!");
            }
            else
            {
                MessageBox.Show("Bad!");
            }
        }

        private bool CanLoginCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3 || Password == null || Password.Length < 5)
                return false;

            return true;
        }

        #endregion
    }
}
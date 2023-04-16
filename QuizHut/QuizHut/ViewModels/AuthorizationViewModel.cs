namespace QuizHut.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.ViewModels.Base;

    internal class AuthorizationViewModel : ViewModel
    {
        private readonly IUserAccountService userAccountService;

        public AuthorizationViewModel(IUserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;

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
            bool loginSuccessful = await userAccountService.LoginAsync(Email, password);

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
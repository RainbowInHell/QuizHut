namespace QuizHut.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResetPasswordViewModel : ViewModel, IResettable
    {
        private readonly IUserAccountService userAccountService;

        public ResetPasswordViewModel(IUserAccountService userAccountService, INavigationService navigationService)
        {
            this.userAccountService = userAccountService;
            this.navigationService = navigationService;

            SendTokenToEmailCommandAsync = new ActionCommandAsync(OnSendTokenToEmailCommandExecuted, CanSendTokenToEmailCommandExecute);
            SubmitTokenCommand = new ActionCommand(OnSubmitTokenCommandExecuted, CanSubmitTokenCommandExecute);
            EnterNewPasswordCommand = new ActionCommand(OnEnterNewPasswordCommandExecuted, CanEnterNewPasswordCommandExecute);
            NavigateAuthorizationViewCommand = new ActionCommand(OnNavigateAuthorizationViewCommandExecuted, CanNavigateAuthorizationViewCommandExecute);
        }

        #region FieldsAndProperties

        private INavigationService navigationService;
        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        private string? email;

        public ResetPasswordViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            SendTokenToEmailCommand = new ActionCommand(OnSendTokenToEmailCommandExecuted, CanSendTokenToEmailCommandExecute);
            SubmitTokenCommand = new ActionCommand(OnSubmitTokenCommandExecuted, CanSubmitTokenCommandExecute);
            EnterNewPasswordCommand = new ActionCommand(OnEnterNewPasswordCommandExecuted, CanEnterNewPasswordCommandExecute);
            NavigateAuthorizationViewCommand = new NavigationCommand(typeof(AuthorizationViewModel), navigationService);
        }

        public string? Email 
        { 
            get => email; 
            set => Set(ref  email, value); 
        }

        private string? token;
        public string? Token 
        { 
            get => token; 
            set => Set(ref token, value);
        }

        private string? password;
        public string? Password 
        { 
            get => password; 
            set => Set(ref password, value); 
        }

        private string? emailErrorMessage;
        public string? EmailErrorMessage 
        { 
            get => emailErrorMessage; 
            set => Set(ref emailErrorMessage, value); 
        }

        private string? tokenErrorMessage;
        public string? TokenErrorMessage 
        { 
            get => tokenErrorMessage; 
            set => Set(ref tokenErrorMessage, value); 
        }

        private string? passwordErrorMessage;
        public string? PasswordErrorMessage 
        { 
            get => passwordErrorMessage; 
            set => Set(ref passwordErrorMessage, value); 
        }

        private bool isEmailEnabled = true;
        public bool IsEmailEnabled 
        { 
            get => isEmailEnabled; 
            set => Set(ref isEmailEnabled, value); 
        }

        private bool isTokenEnabled = false;
        public bool IsTokenEnabled 
        { 
            get => isTokenEnabled;
            set => Set(ref isTokenEnabled, value); 
        }

        private bool isPasswordEnabled = false;
        public bool IsPasswordEnabled 
        { 
            get => isPasswordEnabled; 
            set => Set(ref isPasswordEnabled, value); 
        }

        #endregion

        #region SendTokenToEmailCommand

        public IAsyncCommand SendTokenToEmailCommandAsync { get; }
        
        public bool CanSendTokenToEmailCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3 || IsTokenEnabled == true || IsPasswordEnabled == true)
                return false;

            return true;
        }
        
        public async Task OnSendTokenToEmailCommandExecuted(object p)
        {
            var resetToken = await userAccountService.SendPasswordResetEmail(Email);

            if (resetToken != null)
            {
                var isResetSuccess = await userAccountService.ResetUserPassword(Email, resetToken, "");

                if (isResetSuccess)
                {
                    MessageBox.Show("Good!");
                }
            }
            else
            {

            }
        }

        #endregion

        #region SubmitTokenCommand
        public ICommand SubmitTokenCommand { get; }
        public bool CanSubmitTokenCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Token) || Token.Length < 10 || IsTokenEnabled == false)
                return false;

            return true;
        }
        public void OnSubmitTokenCommandExecuted(object p)
        {
            //submit token functionality
            IsPasswordEnabled = true;
            IsTokenEnabled = false;
        }
        #endregion

        #region EnterNewPasswordCommand
        public ICommand EnterNewPasswordCommand { get; }
        public bool CanEnterNewPasswordCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6 || IsPasswordEnabled == false)
                return false;

            return true;
        }
        public void OnEnterNewPasswordCommandExecuted(object p)
        {
            //enter new password functionality
        }
        #endregion

        #region NavigateAuthorizationViewCommand

        public ICommand NavigateAuthorizationViewCommand { get; }

        #endregion

        public void Reset()
        {
            Email = null;
            Token = null;
            Password = null;

            isEmailEnabled = true;
            isTokenEnabled = false;
            isPasswordEnabled = false;
        }
    }
}

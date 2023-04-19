namespace QuizHut.ViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResetPasswordViewModel : ViewModel, IResettable
    {
        private INavigationService navigationService;

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        private string? email;
        private bool isEmailEnabled = true;

        private string? token;
        private bool isTokenEnabled = false;

        private string? password;
        private bool isPasswordEnabled = false;

        private string? emailErrorMessage;

        private string? tokenErrorMessage;

        private string? passwordErrorMessage;

        public ResetPasswordViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            SendTokenToEmailCommand = new ActionCommand(OnSendTokenToEmailCommandExecuted, CanSendTokenToEmailCommandExecute);
            SubmitTokenCommand = new ActionCommand(OnSubmitTokenCommandExecuted, CanSubmitTokenCommandExecute);
            EnterNewPasswordCommand = new ActionCommand(OnEnterNewPasswordCommandExecuted, CanEnterNewPasswordCommandExecute);
            NavigateAuthorizationViewCommand = new ActionCommand(OnNavigateAuthorizationViewCommandExecuted, CanNavigateAuthorizationViewCommandExecute);
        }

        public string? Email 
        { 
            get => email; 
            set => Set(ref  email, value); 
        }
        public string? Token 
        { 
            get => token; 
            set => Set(ref token, value);
        }
        public string? Password 
        { 
            get => password; 
            set => Set(ref password, value); 
        }
        public string? EmailErrorMessage 
        { 
            get => emailErrorMessage; 
            set => Set(ref emailErrorMessage, value); 
        }
        public string? TokenErrorMessage 
        { 
            get => tokenErrorMessage; 
            set => Set(ref tokenErrorMessage, value); 
        }
        public string? PasswordErrorMessage 
        { 
            get => passwordErrorMessage; 
            set => Set(ref passwordErrorMessage, value); 
        }
        public bool IsEmailEnabled 
        { 
            get => isEmailEnabled; 
            set => Set(ref isEmailEnabled, value); 
        }
        public bool IsTokenEnabled 
        { 
            get => isTokenEnabled;
            set => Set(ref isTokenEnabled, value); 
        }
        public bool IsPasswordEnabled 
        { 
            get => isPasswordEnabled; 
            set => Set(ref isPasswordEnabled, value); 
        }

        #region SendTokenToEmailCommand
        public ICommand SendTokenToEmailCommand { get; }
        public bool CanSendTokenToEmailCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3 || IsTokenEnabled == true || IsPasswordEnabled == true)
                return false;

            return true;
        }
        public void OnSendTokenToEmailCommandExecuted(object p)
        {
            //send token to email functionality
            IsTokenEnabled = true;
            IsEmailEnabled = false;
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

        private void OnNavigateAuthorizationViewCommandExecuted(object p) { NavigationService.NavigateTo<AuthorizationViewModel>(); }

        private bool CanNavigateAuthorizationViewCommandExecute(object p) => true;

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
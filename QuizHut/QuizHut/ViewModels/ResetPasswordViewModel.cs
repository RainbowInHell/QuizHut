namespace QuizHut.ViewModels
{
    using QuizHut.Infrastructure.Commands;
    using QuizHut.ViewModels.Base;
    using System.Windows.Input;

    class ResetPasswordViewModel : ViewModel
    {
        private string? email;
        private bool isEmailEnabled = true;

        private string? token;
        private bool isTokenEnabled = false;

        private string? password;
        private bool isPasswordEnabled = false;

        private string? emailErrorMessage;

        private string? tokenErrorMessage;

        private string? passwordErrorMessage;

        public ResetPasswordViewModel()
        {
            ShowTokenCommand = new ActionCommand(OnShowTokenCommandExecuted, CanShowTokenCommandExecute);
            EnterNewPasswordCommand = new ActionCommand(OnEnterNewPasswordCommandExecuted, CanEnterNewPasswordCommandExecute);
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

        #region ShowTokenCommand
        public ICommand ShowTokenCommand { get; }
        public bool CanShowTokenCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3/* || IsTokenEnabled == true || IsPasswordEnabled == true*/)
                return false;

            return true;
        }
        public void OnShowTokenCommandExecuted(object p)
        {
            //send token to email functionality
            IsTokenEnabled = true;
            IsEmailEnabled = false;
        }
        #endregion

        #region EnterNewPasswordCommand
        public ICommand EnterNewPasswordCommand { get; }
        public bool CanEnterNewPasswordCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Token) || Token.Length < 10 || IsTokenEnabled == false)
                return false;

            return true;
        }
        public void OnEnterNewPasswordCommandExecuted(object p)
        {
            //submit token functionality
            IsPasswordEnabled = true;
            IsTokenEnabled = false;
        }
        #endregion
    }
}
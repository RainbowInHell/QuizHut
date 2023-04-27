namespace QuizHut.ViewModels.LoginViewModels
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using QuizHut.BLL.Dto;
    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DAL.Entities;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResetPasswordViewModel : ViewModel, IResettable
    {
        private readonly IUserAccountService userAccountService;

        private readonly EmailRequestValidator emailValidator;

        private readonly PasswordRequestValidator passwordValidator;

        public ResetPasswordViewModel(
            IUserAccountService userAccountService, 
            INavigationService navigationService,
            EmailRequestValidator emailValidator,
            PasswordRequestValidator passwordValidator)
        {
            this.userAccountService = userAccountService;
            this.navigationService = navigationService;

            this.emailValidator = emailValidator;
            this.passwordValidator = passwordValidator;

            SendTokenToEmailCommandAsync = new ActionCommandAsync(OnSendTokenToEmailCommandExecutedAsync, CanSendTokenToEmailCommandExecute);
            SubmitTokenCommand = new ActionCommand(OnSubmitTokenCommandExecuted, CanSubmitTokenCommandExecute);
            EnterNewPasswordCommandAsync = new ActionCommandAsync(OnEnterNewPasswordCommandExecuted, CanEnterNewPasswordCommandExecute);

            NavigateAuthorizationViewCommand = new NavigationCommand(typeof(AuthorizationViewModel), navigationService);
        }

        #region FieldsAndProperties

        private INavigationService navigationService;

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        private string? email;
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

        private string? newPassword;
        public string? NewPassword 
        { 
            get => newPassword; 
            set => Set(ref newPassword, value); 
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

        private bool isNewPasswordEnabled = false;
        public bool IsNewPasswordEnabled 
        { 
            get => isNewPasswordEnabled; 
            set => Set(ref isNewPasswordEnabled, value); 
        }

        private string ResetToken { get; set; }

        private bool IsEmailValid { get; set; } = true;

        private bool IsPasswordValid { get; set; } = true;

        #endregion

        #region SendTokenToEmailCommand

        public ICommandAsyn SendTokenToEmailCommandAsync { get; }
        
        public bool CanSendTokenToEmailCommandExecute(object p)
        {
            var validationResult = emailValidator.Validate(new EmailRequest { Email = Email });

            if (!IsEmailValid)
            {
                IsEmailValid = true;
                return true;
            }
            if (validationResult.IsValid)
            {
                EmailErrorMessage = null;
                return true;
            }

            var errors = new StringBuilder();

            foreach (var error in validationResult.Errors)
            {
                errors.AppendLine(error.ErrorMessage);
            }

            EmailErrorMessage = errors.ToString();

            return false;
        }
        
        public async Task OnSendTokenToEmailCommandExecutedAsync(object p)
        {
            ResetToken = await userAccountService.SendPasswordResetEmail(Email);

            if (ResetToken != null)
            {
                EmailErrorMessage = "Токен выслан на указанную почту";
                
                IsTokenEnabled = true;
                IsEmailEnabled = false;
            }
            else
            {
                MessageBox.Show("Произошла ошибка. Проверьте адрес электронной почты или попробуйте позже.");
            }
        }

        #endregion

        #region SubmitTokenCommand

        public ICommand SubmitTokenCommand { get; }
        
        public bool CanSubmitTokenCommandExecute(object p)
        {
            if (!IsTokenEnabled)
                return false;

            return true;
        }

        public void OnSubmitTokenCommandExecuted(object p)
        {
            if (ResetToken == Token)
            {
                TokenErrorMessage = "Верный токен";

                IsNewPasswordEnabled = true;
                IsTokenEnabled = false;
            }
            else
            {
                TokenErrorMessage = "Неверный токен";
            }
        }

        #endregion

        #region EnterNewPasswordCommand
        
        public ICommandAsyn EnterNewPasswordCommandAsync { get; }

        public bool CanEnterNewPasswordCommandExecute(object p)
        {
            var validationResult = passwordValidator.Validate(new PasswordRequest { Password = NewPassword });

            if (!IsPasswordValid)
            {
                IsPasswordValid = true;
                return true;
            }
            if (validationResult.IsValid)
            {
                PasswordErrorMessage = null;
                return true;
            }

            var errors = new StringBuilder();

            foreach (var error in validationResult.Errors)
            {
                errors.AppendLine(error.ErrorMessage);
            }

            PasswordErrorMessage = errors.ToString();

            return false;
        }

        public async Task OnEnterNewPasswordCommandExecuted(object p)
        {
            var isResetSuccess = await userAccountService.ResetUserPassword(Email, ResetToken, NewPassword);

            if (isResetSuccess)
            {
                PasswordErrorMessage = "Пароль изменен";
            }
            else
            {
                PasswordErrorMessage = "Непредвиденная ошибка. Пароль не изменен";
            }

            Reset();
        }
        #endregion

        #region NavigateAuthorizationViewCommand

        public ICommand NavigateAuthorizationViewCommand { get; }

        #endregion

        public void Reset()
        {
            Email = null;
            Token = null;
            NewPassword = null;

            IsEmailEnabled = true;
            IsTokenEnabled = false;
            IsNewPasswordEnabled = false;
        }
    }
}
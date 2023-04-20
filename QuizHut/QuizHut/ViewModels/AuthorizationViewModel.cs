namespace QuizHut.ViewModels
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using QuizHut.BLL.Dto;
    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    internal class AuthorizationViewModel : ViewModel, IResettable
    {
        private readonly IUserAccountService userAccountService;

        private readonly LoginRequestValidator validator;

        private bool IsValidLogin { get; set; } = true;

        public AuthorizationViewModel(IUserAccountService userAccountService, INavigationService navigationService, LoginRequestValidator validator)
        {
            this.userAccountService = userAccountService;
            this.navigationService = navigationService;

            this.validator = validator;

            LoginCommandAsync = new ActionCommandAsync(OnLoginCommandExecutedAsync, CanLoginCommandExecute);

            NavigateStudentRegistrationCommand = new ActionCommand(OnNavigateStudentRegistrationCommandExecuted, CanNavigateStudentRegistrationCommandExecute);
            NavigateTeacherRegistrationCommand = new ActionCommand(OnNavigateTeacherRegistrationCommandExecuted, CanNavigateTeacherRegistrationCommandExecute);
            NavigateResetPasswordCommand = new ActionCommand(OnNavigateResetPasswordCommandExecuted, CanNavigateResetPasswordCommandExecute);
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
            set => Set(ref email, value);
        }

        private string? password;
        public string? Password
        {
            get => password;
            set => Set(ref password, value);
        }

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region LoginCommand

        public IAsyncCommand LoginCommandAsync { get; }

        private async Task OnLoginCommandExecutedAsync(object p)
        {
            var loginResponse = await userAccountService.LoginAsync(Email, password);

            IsValidLogin = loginResponse.IsSuccess;

            if (loginResponse.IsSuccess)
            {
                MessageBox.Show("Good!");
            }
            else
            {
                ErrorMessage = loginResponse.Message;
            }
        }

        private bool CanLoginCommandExecute(object p)
        {
            var loginRequest = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var validationResult = validator.Validate(loginRequest);

            if (!IsValidLogin)
            {
                IsValidLogin = true;
                return true;
            }
            if (validationResult.IsValid)
            {
                ErrorMessage = null;
                return true;
            }

            var errors = new StringBuilder();
            foreach (var error in validationResult.Errors)
            {
                errors.AppendLine(error.ErrorMessage);
            }

            ErrorMessage = errors.ToString();

            return false;
        }

        #endregion

        #region NavigateStudentRegistrationCommand

        public ICommand NavigateStudentRegistrationCommand { get; }

        private void OnNavigateStudentRegistrationCommandExecuted(object p) { NavigationService.NavigateTo<StudentRegistrationViewModel>(); }

        private bool CanNavigateStudentRegistrationCommandExecute(object p) => true;

        #endregion

        #region NavigateTeacherRegistrationCommand

        public ICommand NavigateTeacherRegistrationCommand { get; }

        private void OnNavigateTeacherRegistrationCommandExecuted(object p) { NavigationService.NavigateTo<TeacherRegistrationViewModel>(); }

        private bool CanNavigateTeacherRegistrationCommandExecute(object p) => true;

        #endregion

        #region NavigateResetPasswordCommand

        public ICommand NavigateResetPasswordCommand { get; }

        private void OnNavigateResetPasswordCommandExecuted(object p) { NavigationService.NavigateTo<ResetPasswordViewModel>(); }

        private bool CanNavigateResetPasswordCommandExecute(object p) => true;

        #endregion

        public void Reset()
        {
            Email = null;
            Password = null;
        }
    }
}
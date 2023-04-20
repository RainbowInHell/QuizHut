namespace QuizHut.ViewModels
{
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

        private INavigationService navigationService;

        private readonly LoginRequestValidator validator;

        public INavigationService NavigationService 
        { 
            get => navigationService;
            set => Set(ref navigationService, value); 
        }

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

        private string? email;

        private string? password;

        private string? errorMessage;

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

        public string? ErrorMessage 
        { 
            get => errorMessage; 
            set => Set(ref  errorMessage, value); 
        }

        #region LoginCommand

        public IAsyncCommand LoginCommandAsync { get; }

        private async Task OnLoginCommandExecutedAsync(object p)
        {
            var loginResponse = await userAccountService.LoginAsync(Email, password);

            if (loginResponse.IsSuccess)
            {
                MessageBox.Show("Good!");
            }
            else
            {
                MessageBox.Show(loginResponse.Message);
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

            if (validationResult.IsValid)
            {
                return true;
            }

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
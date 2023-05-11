namespace QuizHut.ViewModels.LoginViewModels
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Dto.Requests;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class AuthorizationViewModel : DialogViewModel
    {
        private readonly IUserAccountService userAccountService;

        private readonly LoginRequestValidator validator;

        private readonly IRenavigator mainRenavigator;

        private bool IsLoggedIn { get; set; } = true;

        public AuthorizationViewModel(
            IUserAccountService userAccountService,
            LoginRequestValidator validator,
            IRenavigator studRegisterRenavigator,
            IRenavigator teacherRegisterRenavigator,
            IRenavigator resetPasswordRenavigator,
            IRenavigator mainRenavigator)
        {
            this.userAccountService = userAccountService;
            this.validator = validator;
            this.mainRenavigator = mainRenavigator;

            LoginCommandAsync = new ActionCommandAsync(OnLoginCommandExecutedAsync, CanLoginCommandExecute);

            NavigateStudentRegistrationCommand = new RenavigateCommand(studRegisterRenavigator);
            NavigateTeacherRegistrationCommand = new RenavigateCommand(teacherRegisterRenavigator);
            NavigateResetPasswordCommand = new RenavigateCommand(resetPasswordRenavigator);
        }

        #region FieldsAndProperties

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

        #region Commands

        #region LoginCommand

        public ICommandAsync LoginCommandAsync { get; }

        private bool CanLoginCommandExecute(object p)
        {
            var loginRequest = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var validationResult = validator.Validate(loginRequest);

            if (!IsLoggedIn)
            {
                IsLoggedIn = true;
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

        private async Task OnLoginCommandExecutedAsync(object p)
        {
            IsLoggedIn = await userAccountService.LoginAsync(Email, Password);

            if (IsLoggedIn)
            {
                mainRenavigator.Renavigate();
            }
            else
            {
                ErrorMessage = "Неверная почта или пароль";
            }
        }

        #endregion

        public ICommand NavigateStudentRegistrationCommand { get; }

        public ICommand NavigateTeacherRegistrationCommand { get; }

        public ICommand NavigateResetPasswordCommand { get; }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
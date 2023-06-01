namespace QuizHut.ViewModels.StartViewModels
{
    using System.Linq;
    using System;
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
    using QuizHut.BLL.Helpers.Contracts;

    internal class AuthorizationViewModel : ViewModel
    {
        private readonly IUserAccountService userAccountService;

        private readonly LoginRequestValidator validator;

        private readonly IAccountStore accountStore;

        private readonly IRenavigator teacherMainRenavigator;

        private readonly IRenavigator studentMainRenavigator;

        public AuthorizationViewModel(
            IUserAccountService userAccountService,
            LoginRequestValidator validator,
            IAccountStore accountStore,
            IRenavigator studRegisterRenavigator,
            IRenavigator teacherRegisterRenavigator,
            IRenavigator resetPasswordRenavigator,
            IRenavigator teacherMainRenavigator,
            IRenavigator studentMainRenavigator)
        {
            this.userAccountService = userAccountService;
            this.validator = validator;
            this.accountStore = accountStore;
            this.teacherMainRenavigator = teacherMainRenavigator;
            this.studentMainRenavigator = studentMainRenavigator;

            LoginCommandAsync = new ActionCommandAsync(OnLoginCommandExecutedAsync, CanLoginCommandExecute);

            NavigateStudentRegistrationCommand = new RenavigateCommand(studRegisterRenavigator);
            NavigateTeacherRegistrationCommand = new RenavigateCommand(teacherRegisterRenavigator);
            NavigateResetPasswordCommand = new RenavigateCommand(resetPasswordRenavigator);
        }

        #region FieldsAndProperties

        private string email;
        public string Email
        {
            get => email;
            set => Set(ref email, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateStudentRegistrationCommand { get; }

        public ICommand NavigateTeacherRegistrationCommand { get; }

        public ICommand NavigateResetPasswordCommand { get; }

        #endregion

        #region LoginCommandAsync

        public ICommandAsync LoginCommandAsync { get; }

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
                ErrorMessage = null;
                return true;
            }

            ErrorMessage = string.Join(Environment.NewLine, validationResult.Errors.Select(error => error.ErrorMessage));
            return false;
        }

        private async Task OnLoginCommandExecutedAsync(object p)
        {
            bool isLoggedIn = await userAccountService.LoginAsync(Email, Password);

            if (isLoggedIn)
            {
                if (accountStore.CurrentUserRole == UserRole.Teacher)
                {
                    teacherMainRenavigator.Renavigate();
                }
                else
                {
                    studentMainRenavigator.Renavigate();
                }
            }
            else
            {
                ErrorMessage = "Неверная почта или пароль.";
            }
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
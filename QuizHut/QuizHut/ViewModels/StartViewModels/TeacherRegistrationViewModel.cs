namespace QuizHut.ViewModels.StartViewModels
{
    using System.Threading.Tasks;
    using System;
    using System.Windows.Input;
    using System.Linq;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.DLL.Entities;
    using QuizHut.BLL.Dto.Requests;
    using QuizHut.BLL.Helpers.Contracts;

    internal class TeacherRegistrationViewModel : ViewModel
    {
        private readonly IUserAccountService userAccountService;

        private readonly RegisterRequestValidator validator;

        public TeacherRegistrationViewModel(
            IUserAccountService userAccountService,
            RegisterRequestValidator validator,
            IRenavigator authorizRenavigator,
            IRenavigator studentRegistrRenavigator)
        {
            this.userAccountService = userAccountService;
            this.validator = validator;

            NavigateAuthorizationViewCommand = new RenavigateCommand(authorizRenavigator);
            NavigateStudentRegistrationViewCommand = new RenavigateCommand(studentRegistrRenavigator);

            RegisterCommandAsync = new ActionCommandAsync(OnRegisterCommandExecutedAsync, CanRegisterCommandExecute);
        }

        #region FieldsAndProperties

        private string? email;
        public string? Email
        {
            get => email;
            set => Set(ref email, value);
        }

        private string? firstName;
        public string? FirstName
        {
            get => firstName;
            set => Set(ref firstName, value);
        }

        private string? lastName;
        public string? LastName
        {
            get => lastName;
            set => Set(ref lastName, value);
        }

        private string? password;
        public string? Password
        {
            get => password;
            set => Set(ref password, value);
        }

        private string? repeatPassword;
        public string? RepeatPassword
        {
            get => repeatPassword;
            set => Set(ref repeatPassword, value);
        }

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAuthorizationViewCommand { get; }

        public ICommand NavigateStudentRegistrationViewCommand { get; }

        #endregion

        #region RegisterCommandAsync

        public ICommandAsync RegisterCommandAsync { get; }

        private async Task OnRegisterCommandExecutedAsync(object p)
        {
            var newUser = new ApplicationUser
            {
                UserName = Email,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName
            };

            bool isRegistered = await userAccountService.RegisterAsync(newUser, Password, UserRole.Teacher);

            if (isRegistered)
            {
                ErrorMessage = "Пользователь зарегистрирован.";
            }
            else
            {
                ErrorMessage = "Пользователь с такой почтой уже существует.";
            }
        }

        private bool CanRegisterCommandExecute(object p)
        {
            var registerRequest = new RegisterRequest
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password
            };

            var validationResult = validator.Validate(registerRequest);

            if (validationResult.IsValid)
            {
                ErrorMessage = null;
                return true;
            }

            ErrorMessage = string.Join(Environment.NewLine, validationResult.Errors.Select(error => error.ErrorMessage));
            return false;
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
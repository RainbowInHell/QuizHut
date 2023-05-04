namespace QuizHut.ViewModels.LoginViewModels
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Extensions.DependencyInjection;
    using QuizHut.BLL.Dto;
    using QuizHut.BLL.Dto.DtoValidators;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    internal class AuthorizationViewModel : DialogViewModel, IResettable
    {
        private readonly IUserAccountService userAccountService;
        private readonly LoginRequestValidator validator;
        private readonly IUserDialog userDialog;
        private readonly IRenavigator mainRenavigator;

        private IServiceProvider ServiceProvider { get; set; }
        private bool IsLoggedIn { get; set; } = true;

        public AuthorizationViewModel(
            IUserAccountService userAccountService,
            LoginRequestValidator validator,
            IUserDialog userDialog,
            IRenavigator studRegisterRenavigator,
            IRenavigator teacherRegisterRenavigator,
            IRenavigator resetPasswordRenavigator,
            IRenavigator mainRenavigator,
            IServiceProvider serviceProvider)
        {
            this.userAccountService = userAccountService;
            this.userDialog = userDialog;
            this.validator = validator;
            this.mainRenavigator = mainRenavigator;

            LoginCommandAsync = new ActionCommandAsync(OnLoginCommandExecutedAsync, CanLoginCommandExecute);

            NavigateStudentRegistrationCommand = new RenavigateCommand(studRegisterRenavigator);
            NavigateTeacherRegistrationCommand = new RenavigateCommand(teacherRegisterRenavigator);
            NavigateResetPasswordCommand = new RenavigateCommand(resetPasswordRenavigator);
            ServiceProvider = serviceProvider;
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
                MessageBox.Show("Успех!");
            }
            else
            {
                ErrorMessage = "Неверная почта или пароль";
            }

            mainRenavigator.Renavigate();
        }

        #endregion

        #region NavigateStudentRegistrationCommand

        public ICommand NavigateStudentRegistrationCommand { get; }

        #endregion

        #region NavigateTeacherRegistrationCommand

        public ICommand NavigateTeacherRegistrationCommand { get; }

        #endregion

        #region NavigateResetPasswordCommand

        public ICommand NavigateResetPasswordCommand { get; }

        #endregion

        public void Reset()
        {
            Email = null;
            Password = null;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
namespace QuizHut.ViewModels.LoginViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DAL.Entities;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal class StudentRegistrationViewModel : ViewModel
    {
        private readonly IUserAccountService userAccountService;

        private INavigationService navigationService;

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        public StudentRegistrationViewModel(IUserAccountService userAccountService, INavigationService navigationService)
        {
            this.userAccountService = userAccountService;
            this.navigationService = navigationService;

            RegisterCommandAsync = new ActionCommandAsync(OnRegisterCommandExecutedAsync, CanRegisterCommandExecute);

            NavigateAuthorizationViewCommand = new NavigationCommand(typeof(AuthorizationViewModel), navigationService);
            NavigateTeacherRegistrationViewCommand = new NavigationCommand(typeof(TeacherRegistrationViewModel), navigationService);
        }

        private string? email;

        private string? firstName;

        private string? lastName;

        private string? password;

        private string? repeatPassword;

        public string? Email
        {
            get => email;
            set => Set(ref email, value);
        }

        public string? FirstName
        {
            get => firstName;
            set => Set(ref firstName, value);
        }

        public string? LastName
        {
            get => lastName;
            set => Set(ref lastName, value);
        }

        public string? Password
        {
            get => password;
            set => Set(ref password, value);
        }

        public string? RepeatPassword
        {
            get => repeatPassword;
            set => Set(ref repeatPassword, value);
        }

        #region RegisterCommand

        public IAsyncCommand RegisterCommandAsync { get; }

        private async Task OnRegisterCommandExecutedAsync(object p)
        {
            var newUser = new ApplicationUser
            {
                UserName = Email,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName
            };

            bool registerSuccessful = await userAccountService.RegisterAsync(newUser, Password);

            if (registerSuccessful)
            {
                MessageBox.Show("Good!");
            }
            else
            {
                MessageBox.Show("Bad!");
            }
        }

        private bool CanRegisterCommandExecute(object p)
        {
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3 || Password == null || Password.Length < 5)
                return false;

            return true;
        }

        #endregion

        #region NavigateAuthorizationViewCommand

        public ICommand NavigateAuthorizationViewCommand { get; }

        #endregion

        #region NavigateTeacherRegistrationViewCommand

        public ICommand NavigateTeacherRegistrationViewCommand { get; }

        #endregion
    }
}
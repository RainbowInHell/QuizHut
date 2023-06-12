namespace QuizHut.ViewModels.MainViewModels
{
    using System.IO;
    using System.Reflection;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;
    using QuizHut.ViewModels.Factory;

    class MainViewModel : ViewModel
    {
        private readonly IUserAccountService userAccountService;

        private readonly INavigationService navigationService;

        private readonly IAccountStore accountStore;

        private readonly ISharedDataStore sharedDataStore;

        public MainViewModel(
            IUserAccountService userAccountService, 
            INavigationService navigationService,
            IAccountStore accountStore,
            ISharedDataStore sharedDataStore, 
            IViewModelFactory traderViewModelFactory)
        {
            this.userAccountService = userAccountService;
            this.navigationService = navigationService;
            this.accountStore = accountStore;
            this.sharedDataStore = sharedDataStore;

            navigationService.StateChanged += NavigationService_StateChanged;
            accountStore.StateChanged += UserAccountService_StateChanged;

            NavigateProfileCommand = new ActionCommand(p => ProfileNavigate());

            NavigationCommand = new NavigationCommand(navigationService, traderViewModelFactory);
            NavigationCommand.Execute(ViewType.Authorization);
            
            LogoutCommand = new ActionCommand(OnLogoutCommandExecuted);
            ShowHelpCommand = new ActionCommand(OnShowHelpCommandExecuted);
        }

        #region Fields and properties

        public ViewModel CurrentView => navigationService.CurrentView;

        public UserRole CurrentUserRole => accountStore.CurrentUserRole;

        public ApplicationUser CurrentUser
        {
            get
            {
                if (accountStore.CurrentUser != null)
                {
                    sharedDataStore.CurrentUser = accountStore.CurrentUser;
                    return accountStore.CurrentUser;
                }

                return null;
            }
        }

        private string title;
        public string Title 
        { 
            get => title;
            set => Set(ref title, value);
        }

        private IconChar iconChar;
        public IconChar IconChar 
        { 
            get => iconChar;
            set => Set(ref iconChar, value);
        }

        private string? selectedOption = "Home";
        public string? SelectedOption
        {
            get => selectedOption;
            set => Set(ref selectedOption, value);
        }

        #endregion

        #region NavigationCommands

        public NavigationCommand NavigationCommand { get; }

        public ICommand NavigateProfileCommand { get; }

        #endregion

        #region LogoutCommand
        public ICommand LogoutCommand { get; }
        private void OnLogoutCommandExecuted(object p)
        {
            userAccountService.Logout();
            NavigationCommand.Execute(ViewType.Authorization);
        }

        #endregion

        #region ShowHelpCommand
        public ICommand ShowHelpCommand { get; }
        private void OnShowHelpCommandExecuted(object p)
        {
            string currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            System.Windows.Forms.Help.ShowHelp(null, Path.Combine(currentDirectoryPath, @"QuizzesHutHelp.chm"));
        }

        #endregion

        private void ShowingContent()
        {
            if (CurrentView is IView menuView)
            {
                Title = menuView.GetType().GetProperty("Title").GetValue(menuView).ToString();

                var iconCharProperty = menuView.GetType().GetProperty("IconChar");
                
                if (iconCharProperty != null)
                {
                    IconChar = (IconChar)iconCharProperty.GetValue(menuView);
                }
            }
        }

        private void UserAccountService_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentUserRole));
            OnPropertyChanged(nameof(CurrentUser));
        }

        private void NavigationService_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
            ShowingContent();
        }

        private void ProfileNavigate()
        {
            NavigationCommand.Execute(ViewType.UserProfile);
        }

        public override void Dispose()
        {
            navigationService.StateChanged -= NavigationService_StateChanged;
            accountStore.StateChanged -= UserAccountService_StateChanged;

            base.Dispose();
        }
    }
}
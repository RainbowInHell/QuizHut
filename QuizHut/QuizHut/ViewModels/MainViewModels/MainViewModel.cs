namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Factory;
    
    class MainViewModel : DialogViewModel
    {
        public MainViewModel(INavigationService navigationService, ISimpleTraderViewModelFactory traderViewModelFactory, IUserAccountService userAccountService)
        {
            this.navigationService = navigationService;
            this.traderViewModelFactory = traderViewModelFactory;
            this.userAccountService = userAccountService;

            navigationService.StateChanged += NavigationService_StateChanged;
            userAccountService.StateChanged += UserAccountService_StateChanged;

            NavigationCommand = new NavigationCommand(navigationService, traderViewModelFactory);
            NavigationCommand.Execute(ViewType.Authorization);

            LogoutCommand = new ActionCommand(OnLogoutCommandExecuted);
        }

        private void UserAccountService_StateChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private void NavigationService_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }

        #region Fields and properties

        private readonly ISimpleTraderViewModelFactory traderViewModelFactory;

        private readonly INavigationService navigationService;

        private readonly IUserAccountService userAccountService;

        public ViewModel CurrentView => navigationService.CurrentView;

        public bool IsLoggedIn => userAccountService.IsLoggedIn;

        //private bool isLoggedIn = false;
        //public bool IsLoggedIn 
        //{
        //    get => isLoggedIn; 
        //    set => Set(ref  isLoggedIn, value);
        //}

        private string caption;
        public string Caption 
        { 
            get => caption; 
            set => Set(ref  caption, value); 
        }

        private IconChar iconChar;
        public IconChar IconChar 
        { 
            get => iconChar; 
            set => Set(ref iconChar, value); 
        }

        private string selectedOption = "Home";
        public string? SelectedOption 
        { 
            get => selectedOption; 
            set => Set(ref selectedOption, value); 
        }

        #endregion

        #region Commands

        public NavigationCommand NavigationCommand { get; }

        #region LogoutCommand
        public ICommand LogoutCommand { get; } 
        private void OnLogoutCommandExecuted(object p)
        {
            userAccountService.Logout();
            NavigationCommand.Execute(ViewType.Authorization);
        }

        #endregion

        #endregion

        private void ShowingContent<T>() where T : ViewModel
        {
            Caption = typeof(T).GetProperty("Title").GetValue(null).ToString();
            IconChar = (IconChar)typeof(T).GetProperty("IconChar").GetValue(null);
        }

        public override void Dispose()
        {
            navigationService.StateChanged -= NavigationService_StateChanged;
            userAccountService.StateChanged -= UserAccountService_StateChanged;

            base.Dispose();
        }
    }
}
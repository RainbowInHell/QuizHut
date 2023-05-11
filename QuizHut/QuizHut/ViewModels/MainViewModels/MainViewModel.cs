namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;
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
            ShowingContent();
        }

        #region Fields and properties

        private readonly ISimpleTraderViewModelFactory traderViewModelFactory;

        private readonly INavigationService navigationService;

        private readonly IUserAccountService userAccountService;

        public ViewModel CurrentView => navigationService.CurrentView;

        public bool IsLoggedIn => userAccountService.IsLoggedIn;
        public bool IsLoggedIn => true;

        private string title = HomeViewModel.Title;
        public string Title 
        { 
            get => title;
            set => Set(ref title, value);
        }

        private IconChar iconChar = HomeViewModel.IconChar;
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

        private void ShowingContent()
        {
            if(CurrentView is IMenuView menuView)
            {
                Title = menuView.GetType().GetProperty("Title").GetValue(null).ToString();
                IconChar = (IconChar)menuView.GetType().GetProperty("IconChar").GetValue(null);
            }
        }

        public override void Dispose()
        {
            navigationService.StateChanged -= NavigationService_StateChanged;
            userAccountService.StateChanged -= UserAccountService_StateChanged;

            base.Dispose();
        }
    }
}
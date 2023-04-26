namespace QuizHut.ViewModels
{
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    
    class MainViewModel : ViewModel
    {
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            OnShowHomeViewCommandExecuted(null);

            ShowHomeViewCommand = new ActionCommand(OnShowHomeViewCommandExecuted);
            ShowUserProfileViewCommand = new ActionCommand(OnShowUserProfileViewCommandExecuted);
        }

        #region Fields and properties

        private INavigationService navigationService;
        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

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

        #endregion

        #region Commands

        #region ShowHomeViewCommand

        public ICommand ShowHomeViewCommand { get; }
        private void OnShowHomeViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<HomeViewModel>();
            Caption = HomeViewModel.Title;
            IconChar = HomeViewModel.IconChar;
        }

        #endregion

        #region ShowUserProfileViewCommand

        public ICommand ShowUserProfileViewCommand { get; }
        private void OnShowUserProfileViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<UserProfileViewModel>();
            Caption = UserProfileViewModel.Title;
            IconChar = UserProfileViewModel.IconChar;
        }

        #endregion

        #endregion
    }
}
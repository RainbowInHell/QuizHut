namespace QuizHut.ViewModels.LoginViewModels
{
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Factory;
    using System.Windows.Input;

    class LoginViewModel : DialogViewModel
    {
        private readonly ISimpleTraderViewModelFactory viewModelFactory;
        private INavigationService navigationService;
        private readonly IUserDialogService userDialog;

        public ViewModel CurrentView => navigationService.CurrentView;

        public LoginViewModel(INavigationService navigationService, IUserDialog userDialog, ISimpleTraderViewModelFactory viewModelFactory = null)
        {

            this.userDialog = userDialog;
            this.viewModelFactory = viewModelFactory;
            this.navigationService = navigationService;

            navigationService.StateChanged += NavigationService_StateChanged;

            NavigationCommand = new NavigationCommand(navigationService, viewModelFactory);
            NavigationCommand.Execute(ViewType.Authorization);
        }

        private void NavigationService_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }

        public ICommand NavigationCommand { get; }

        public override void Dispose()
        {
            navigationService.StateChanged -= NavigationService_StateChanged;

            base.Dispose();
        }
    }
}
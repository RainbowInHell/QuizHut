namespace QuizHut.ViewModels.LoginViewModels
{
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class LoginViewModel : DialogViewModel
    {
        private INavigationService navigationService;
        private readonly IUserDialog userDialog;

        public LoginViewModel(INavigationService navigationService, IUserDialog userDialog)
        {
            this.navigationService = navigationService;
            this.userDialog = userDialog;

            NavigationService.NavigateTo<AuthorizationViewModel>();
        }

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }
    }
}
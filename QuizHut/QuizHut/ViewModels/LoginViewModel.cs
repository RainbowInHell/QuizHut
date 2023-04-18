namespace QuizHut.ViewModels
{
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class LoginViewModel:ViewModel
    {
        private INavigationService navigationService;

        public LoginViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }
    }
}
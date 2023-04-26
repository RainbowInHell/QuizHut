namespace QuizHut.ViewModels
{
    using QuizHut.Infrastructure.Services.Contracts;

    using QuizHut.ViewModels.Base;

    class MainViewModel : ViewModel
    {
        private INavigationService navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            NavigationService.NavigateTo<HomeViewModel>();
        }

        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }
    }
}
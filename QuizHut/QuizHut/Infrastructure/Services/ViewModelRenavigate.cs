namespace QuizHut.Infrastructure.Services
{
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class ViewModelRenavigate<TViewModel> : IRenavigator where TViewModel : ViewModel
    {
        private readonly INavigationService navigationService;

        private readonly CreateViewModel<TViewModel> createViewModel;

        public ViewModelRenavigate(INavigationService navigationService, CreateViewModel<TViewModel> createViewModel)
        {
            this.navigationService = navigationService;
            this.createViewModel = createViewModel;
        }
        public void Renavigate()
        {
            navigationService.CurrentView = createViewModel();
        }
    }
}
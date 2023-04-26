namespace QuizHut.ViewModels.MainViewModels
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
            ShowResultsViewCommand = new ActionCommand(OnShowResultsViewCommandExecuted);
            ShowEventsViewCommand = new ActionCommand(OnShowEventsViewCommandExecuted);
            ShowGroupsViewCommand = new ActionCommand(OnShowGroupsViewCommandExecuted);
            ShowCategoriesViewCommand = new ActionCommand(OnShowCategoriesViewCommandExecuted);
            ShowQuizzesViewCommand = new ActionCommand(OnShowQuizzesViewCommandExecuted);
            ShowStudentsViewCommand = new ActionCommand(OnShowStudentsViewCommandExecuted);
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

        private string selectedOption = "Home";
        public string? SelectedOption 
        { 
            get => selectedOption; 
            set => Set(ref  selectedOption, value); 
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
            SelectedOption = null;
        }

        #endregion

        #region ShowResultsViewCommand

        public ICommand ShowResultsViewCommand { get; }
        private void OnShowResultsViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<ResultsViewModel>();
            Caption = ResultsViewModel.Title;
            IconChar = ResultsViewModel.IconChar;
        }

        #endregion

        #region ShowEventsViewCommand

        public ICommand ShowEventsViewCommand { get; }
        private void OnShowEventsViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<EventsViewModel>();
            Caption = EventsViewModel.Title;
            IconChar = EventsViewModel.IconChar;
        }

        #endregion

        #region ShowGroupsViewCommand

        public ICommand ShowGroupsViewCommand { get; }
        private void OnShowGroupsViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<GroupsViewModel>();
            Caption = GroupsViewModel.Title;
            IconChar = GroupsViewModel.IconChar;
        }

        #endregion

        #region ShowCategoriesViewCommand

        public ICommand ShowCategoriesViewCommand { get; }
        private void OnShowCategoriesViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<CategoriesViewModel>();
            Caption = CategoriesViewModel.Title;
            IconChar = CategoriesViewModel.IconChar;
        }

        #endregion

        #region ShowQuizzesViewCommand

        public ICommand ShowQuizzesViewCommand { get; }
        private void OnShowQuizzesViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<QuizzesViewModel>();
            Caption = QuizzesViewModel.Title;
            IconChar = QuizzesViewModel.IconChar;
        }

        #endregion

        #region ShowStudentsViewCommand

        public ICommand ShowStudentsViewCommand { get; }

        private void OnShowStudentsViewCommandExecuted(object p)
        {
            NavigationService.NavigateTo<StudentsViewModel>();
            Caption = StudentsViewModel.Title;
            IconChar = StudentsViewModel.IconChar;
        }

        #endregion

        #endregion


    }
}
namespace QuizHut.ViewModels.MainViewModels
{
    using System;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    
    class MainViewModel : DialogViewModel
    {
        public MainViewModel(INavigationService navigationService, IUserDialog userDialog)
        {
            this.navigationService = navigationService;
            this.userDialog = userDialog;

            ShowingContent<HomeViewModel>();

            ShowHomeViewCommand = new ActionCommand(p => ShowingContent<HomeViewModel>());
            ShowUserProfileViewCommand = new ActionCommand(OnShowUserProfileViewCommandExecuted);
            ShowResultsViewCommand = new ActionCommand(p => ShowingContent<ResultsViewModel>());
            ShowEventsViewCommand = new ActionCommand(p => ShowingContent<EventsViewModel>());
            ShowGroupsViewCommand = new ActionCommand(p => ShowingContent<GroupsViewModel>());
            ShowCategoriesViewCommand = new ActionCommand(p => ShowingContent<CategoriesViewModel>());
            ShowQuizzesViewCommand = new ActionCommand(p => ShowingContent<QuizzesViewModel>());
            ShowStudentsViewCommand = new ActionCommand(p => ShowingContent<StudentsViewModel>());

            LogoutCommand = new ActionCommand(OnLogoutCommandExecuted);
        }

        #region Fields and properties

        private readonly IUserDialog userDialog;

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
            set => Set(ref selectedOption, value); 
        }

        #endregion

        #region Commands

        public ICommand ShowHomeViewCommand { get; }

        #region ShowUserProfileViewCommand

        public ICommand ShowUserProfileViewCommand { get; }
        private void OnShowUserProfileViewCommandExecuted(object p)
        {
            ShowingContent<UserProfileViewModel>();
            SelectedOption = null;
        }

        #endregion

        public ICommand ShowResultsViewCommand { get; }

        public ICommand ShowEventsViewCommand { get; }

        public ICommand ShowGroupsViewCommand { get; }

        public ICommand ShowCategoriesViewCommand { get; }

        public ICommand ShowQuizzesViewCommand { get; }

        public ICommand ShowStudentsViewCommand { get; }

        #region LogoutCommand
        public ICommand LogoutCommand { get; }
        private void OnLogoutCommandExecuted(object p)
        {
            userDialog.OpenLoginView();
            OnDialogComplete(EventArgs.Empty);
        } 
        #endregion

        #endregion

        private void ShowingContent<T>() where T : ViewModel
        {
            NavigationService.NavigateTo<T>();
            Caption = typeof(T).GetProperty("Title").GetValue(null).ToString();
            IconChar = (IconChar)typeof(T).GetProperty("IconChar").GetValue(null);
        }
    }
}
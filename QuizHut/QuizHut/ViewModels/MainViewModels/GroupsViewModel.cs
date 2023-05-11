namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class GroupsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Группы";
        
        public static IconChar IconChar { get; } = IconChar.PeopleGroup;

        private readonly IGroupsService groupsService;

        public GroupsViewModel(IGroupsService groupsService)
        {
            this.groupsService = groupsService;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
            GoToCreateGroupCommandAsync = new ActionCommandAsync(OnGoToCreateGroupCommandExecutedAsync, CanGoToCreateGroupCommandExecute);
            GoToEditGroupNameCommandAsync = new ActionCommandAsync(OnGoToEditGroupNameCommandAsyncExecute, CanGoToEditGroupNameCommandAsyncExecute);
            GoToGroupSettingsCommandAsync = new ActionCommandAsync(OnGoToGroupSettingsCommandAsyncExecute, CanGoToGroupSettingsCommandAsyncExecute);
            DeleteGroupCommandAsync = new ActionCommandAsync(OnDeleteGroupCommandExecutedAsync, CanDeleteGroupCommandExecute);
        }

        #region FieldsAndProperties

        public ObservableCollection<GroupListViewModel> groups;
        public ObservableCollection<GroupListViewModel> Groups
        {
            get => groups;
            set => Set(ref groups, value);
        }

        private GroupListViewModel selectedGroup;
        public GroupListViewModel SelectedGroup
        {
            get => selectedGroup;
            set => Set(ref selectedGroup, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadGroupsData();
        }

        #endregion

        #region SearchCommand

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => true;

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadGroupsData(searchCriteria: "Name", searchText: SearchText);
        }

        #endregion

        #region GoToCreateGroupCommand

        public ICommandAsync GoToCreateGroupCommandAsync { get; }

        private bool CanGoToCreateGroupCommandExecute(object p) => true;

        private async Task OnGoToCreateGroupCommandExecutedAsync(object p)
        {
        }

        #endregion

        #region GoToEditGroupNameCommand

        public ICommandAsync GoToEditGroupNameCommandAsync { get; }

        private bool CanGoToEditGroupNameCommandAsyncExecute(object p) => true;

        private async Task OnGoToEditGroupNameCommandAsyncExecute(object p)
        {
        }

        #endregion

        #region GoToGroupSettingsCommand

        public ICommandAsync GoToGroupSettingsCommandAsync { get; }

        private bool CanGoToGroupSettingsCommandAsyncExecute(object p) => true;

        private async Task OnGoToGroupSettingsCommandAsyncExecute(object p)
        {
        }

        #endregion

        #region DeleteGroupCommand

        public ICommandAsync DeleteGroupCommandAsync { get; }

        private bool CanDeleteGroupCommandExecute(object p) => true;

        private async Task OnDeleteGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteAsync(SelectedGroup.Id);

            await LoadGroupsData();
        }

        #endregion

        private async Task LoadGroupsData(
            //remove
            string creatorId = "aa0f4db3-d1a4-4dbe-a1a5-45313b2f88c3",
            string eventId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var groups = await groupsService.GetAllAsync<GroupListViewModel>(creatorId, eventId, searchCriteria, searchText);

            Groups = new(groups);
        }

        public GroupsViewModel(IRenavigator createGroupRenavigator) 
        {
            NavigateCreateGroupCommand = new RenavigateCommand(createGroupRenavigator);
        }

        #region Commands

        public ICommand NavigateCreateGroupCommand { get; }

        #endregion
    }
}
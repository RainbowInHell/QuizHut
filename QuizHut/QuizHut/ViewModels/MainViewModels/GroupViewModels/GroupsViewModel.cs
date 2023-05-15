namespace QuizHut.ViewModels.MainViewModels.GroupViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class GroupsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Группы";

        public static IconChar IconChar { get; } = IconChar.PeopleGroup;

        private readonly IGroupsService groupsService;

        private readonly ISharedDataStore sharedDataStore;

        public GroupsViewModel(
            IGroupsService groupsService,
            ISharedDataStore sharedDataStore,
            IRenavigator groupActionsRenavigator,
            IRenavigator groupSettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.groupsService = groupsService;
            this.sharedDataStore = sharedDataStore;

            NavigateCreateGroupCommand = new RenavigateCommand(groupActionsRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditGroupCommand = new RenavigateCommand(groupActionsRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateGroupSettingsCommand = new RenavigateCommand(groupSettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
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
            get
            {
                sharedDataStore.SelectedGroupId = selectedGroup is null ? null : selectedGroup.Id;
                return selectedGroup;
            }
            set => Set(ref selectedGroup, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateCreateGroupCommand { get; }

        public ICommand NavigateEditGroupCommand { get; }

        public ICommand NavigateGroupSettingsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadGroupsData();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => true;

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadGroupsData("Name", SearchText);
        }

        #endregion

        #region DeleteGroupCommandAsync

        public ICommandAsync DeleteGroupCommandAsync { get; }

        private bool CanDeleteGroupCommandExecute(object p) => true;

        private async Task OnDeleteGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteAsync(SelectedGroup.Id);

            await LoadGroupsData();
        }

        #endregion

        private async Task LoadGroupsData(string searchCriteria = null, string searchText = null)
        {
            var groups = await groupsService.GetAllGroupsAsync<GroupListViewModel>(AccountStore.CurrentAdminId, searchCriteria: searchCriteria, searchText: searchText);

            Groups = new(groups);
        }
    }
}
namespace QuizHut.ViewModels.MainViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;
    
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
            IRenavigator createGroupRenavigator, 
            IGroupSettingsTypeService groupSettingsTypeService,
            ISharedDataStore sharedDataStore)
        {
            this.groupsService = groupsService;
            this.sharedDataStore = sharedDataStore;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
            DeleteGroupCommandAsync = new ActionCommandAsync(OnDeleteGroupCommandExecutedAsync, CanDeleteGroupCommandExecute);

            NavigateCreateGroupCommand = new RenavigateCommand(createGroupRenavigator, GroupViewType.Create, groupSettingsTypeService);
            NavigateEditGroupCommand = new RenavigateCommand(createGroupRenavigator, GroupViewType.Edit, groupSettingsTypeService);
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

        #region DeleteGroupCommand

        public ICommandAsync DeleteGroupCommandAsync { get; }

        private bool CanDeleteGroupCommandExecute(object p) => true;

        private async Task OnDeleteGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteAsync(SelectedGroup.Id);

            await LoadGroupsData();
        }

        #endregion

        public ICommand NavigateCreateGroupCommand { get; }

        public ICommand NavigateEditGroupCommand { get; }

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
    }
}
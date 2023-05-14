namespace QuizHut.ViewModels.MainViewModels.GroupViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class GroupActionsViewModel : ViewModel
    {
        private readonly IGroupsService groupsService;

        private readonly IStudentsService studentService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IGroupSettingsTypeService groupSettingsTypeService;

        public GroupActionsViewModel(
            IGroupsService groupsService,
            IStudentsService studentService,
            ISharedDataStore sharedDataStore,
            IRenavigator groupRenavigator,
            IRenavigator groupSettingRenavigator,
            IGroupSettingsTypeService groupSettingsTypeService)
        {
            this.studentService = studentService;
            this.groupsService = groupsService;
            this.groupSettingsTypeService = groupSettingsTypeService;
            this.sharedDataStore = sharedDataStore;

            groupSettingsTypeService.StateChanged += GroupSettingsTypeService_StateChanged;

            NavigateGroupCommand = new RenavigateCommand(groupRenavigator);
            NavigateGroupSettingsCommand = new RenavigateCommand(groupSettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            CreateGroupCommandAsync = new ActionCommandAsync(OnCreateGroupCommandExecutedAsync, CanCreateGroupCommandExecute);
            UpdateGroupNameCommandAsync = new ActionCommandAsync(OnUpdateGroupNameCommandExecutedAsync, CanUpdateGroupNameCommandExecute);
            AssignStudentsToGroupCommandAsync = new ActionCommandAsync(OnAssignStudentsToGroupCommandExecute, CanAssignStudentsToGroupCommandExecute);
        }

        #region Fields and properties

        public GroupViewType? GroupViewType => groupSettingsTypeService.GroupViewType;

        private string groupNameToCreate;
        public string GroupNameToCreate
        {
            get => groupNameToCreate;
            set => Set(ref groupNameToCreate, value);
        }

        public ObservableCollection<StudentViewModel> students;
        public ObservableCollection<StudentViewModel> Students
        {
            get => students;
            set => Set(ref students, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateGroupCommand { get; }

        public ICommand NavigateGroupSettingsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadStudentsData();
        }

        #endregion

        #region CreateGroupCommand

        public ICommandAsync CreateGroupCommandAsync { get; }

        private bool CanCreateGroupCommandExecute(object p) => !string.IsNullOrEmpty(GroupNameToCreate);

        private async Task OnCreateGroupCommandExecutedAsync(object p)
        {
            await groupsService.CreateGroupAsync(GroupNameToCreate, AccountStore.CurrentAdminId);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        #region UpdateGroupNameCommand

        public ICommandAsync UpdateGroupNameCommandAsync { get; }

        private bool CanUpdateGroupNameCommandExecute(object p) => !string.IsNullOrEmpty(GroupNameToCreate);

        private async Task OnUpdateGroupNameCommandExecutedAsync(object p)
        {
            await groupsService.UpdateNameAsync(sharedDataStore.SelectedGroupId, GroupNameToCreate);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        #region AssignStudentsToGroupCommandAsync

        public ICommandAsync AssignStudentsToGroupCommandAsync { get; }

        private bool CanAssignStudentsToGroupCommandExecute(object p) => true;

        private async Task OnAssignStudentsToGroupCommandExecute(object p)
        {
            var selectedStudentIds = Students.Where(s => s.IsAssigned).Select(s => s.Id).ToList();

            if (selectedStudentIds.Any())
            {
                await groupsService.AssignStudentsToGroupAsync(sharedDataStore.SelectedGroupId, selectedStudentIds);
            }

            NavigateGroupSettingsCommand.Execute(p);
        }

        #endregion

        private async Task LoadStudentsData(string searchCriteria = null, string searchText = null)
        {
            var students = await studentService.GetAllStudentsAsync<StudentViewModel>(AccountStore.CurrentAdminId, sharedDataStore.SelectedGroupId, searchCriteria, searchText);

            Students = new(students);
        }

        private void GroupSettingsTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(GroupViewType));
        }

        public override void Dispose()
        {
            groupSettingsTypeService.StateChanged -= GroupSettingsTypeService_StateChanged;

            base.Dispose();
        }
    }
}
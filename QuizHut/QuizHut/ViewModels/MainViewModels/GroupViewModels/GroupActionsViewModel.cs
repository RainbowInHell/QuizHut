namespace QuizHut.ViewModels.MainViewModels.GroupViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common.Enumerations;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class GroupActionsViewModel : ViewModel
    {
        private readonly IGroupsService groupsService;

        private readonly IEventsService eventsService;

        private readonly IStudentsService studentService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public GroupActionsViewModel(
            IGroupsService groupsService,
            IEventsService eventsService,
            IStudentsService studentService,
            ISharedDataStore sharedDataStore,
            IRenavigator groupRenavigator,
            IRenavigator groupSettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.groupsService = groupsService;
            this.eventsService = eventsService;
            this.studentService = studentService;
            this.viewDisplayTypeService = viewDisplayTypeService;
            this.sharedDataStore = sharedDataStore;

            viewDisplayTypeService.StateChanged += ViewDisplayTypeService_StateChanged;

            NavigateGroupCommand = new RenavigateCommand(groupRenavigator);
            NavigateGroupSettingsCommand = new RenavigateCommand(groupSettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            CreateGroupCommandAsync = new ActionCommandAsync(OnCreateGroupCommandExecutedAsync, CanCreateGroupCommandExecute);
            UpdateGroupNameCommandAsync = new ActionCommandAsync(OnUpdateGroupNameCommandExecutedAsync, CanUpdateGroupNameCommandExecute);
            AssignStudentsToGroupCommandAsync = new ActionCommandAsync(OnAssignStudentsToGroupCommandExecute, CanAssignStudentsToGroupCommandExecute);
            AssignEventsToGroupCommandAsync = new ActionCommandAsync(OnAssignEventsToGroupCommandExecute, CanAssignEventsToGroupCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? ViewDisplayType => viewDisplayTypeService.ViewDisplayType;

        public ObservableCollection<StudentViewModel> students;
        public ObservableCollection<StudentViewModel> Students
        {
            get => students;
            set => Set(ref students, value);
        }

        public ObservableCollection<EventsAssignViewModel> events;
        public ObservableCollection<EventsAssignViewModel> Events
        {
            get => events;
            set => Set(ref events, value);
        }

        private string groupNameToCreate;
        public string GroupNameToCreate
        {
            get => groupNameToCreate;
            set => Set(ref groupNameToCreate, value);
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

            await LoadEventsData();
        }

        #endregion

        #region CreateGroupCommandAsync

        public ICommandAsync CreateGroupCommandAsync { get; }

        private bool CanCreateGroupCommandExecute(object p) => !string.IsNullOrEmpty(GroupNameToCreate);

        private async Task OnCreateGroupCommandExecutedAsync(object p)
        {
            await groupsService.CreateGroupAsync(GroupNameToCreate, AccountStore.CurrentAdminId);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        #region UpdateGroupNameCommandAsync

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

        #region AssignEventsToGroupCommandAsync

        public ICommandAsync AssignEventsToGroupCommandAsync { get; }

        private bool CanAssignEventsToGroupCommandExecute(object p) => true;

        private async Task OnAssignEventsToGroupCommandExecute(object p)
        {
            var selectedEventIds = Events.Where(s => s.IsAssigned).Select(s => s.Id).ToList();

            if (selectedEventIds.Any())
            {
                await groupsService.AssignEventsToGroupAsync(sharedDataStore.SelectedGroupId, selectedEventIds);
            }

            NavigateGroupSettingsCommand.Execute(p);
        }

        #endregion

        private async Task LoadStudentsData()
        {
            var students = await studentService.GetAllStudentsAsync<StudentViewModel>(AccountStore.CurrentAdminId, sharedDataStore.SelectedGroupId);

            Students = new(students);
        }

        private async Task LoadEventsData()
        {
            var events = await eventsService.GetAllFilteredByStatusAndGroupAsync<EventsAssignViewModel>(Status.Ended, sharedDataStore.SelectedGroupId, AccountStore.CurrentAdminId);

            Events = new(events);
        }

        private void ViewDisplayTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(ViewDisplayType));
        }

        public override void Dispose()
        {
            viewDisplayTypeService.StateChanged -= ViewDisplayTypeService_StateChanged;

            base.Dispose();
        }
    }
}
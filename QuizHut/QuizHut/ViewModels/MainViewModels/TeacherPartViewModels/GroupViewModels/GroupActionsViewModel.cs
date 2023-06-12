namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.GroupViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
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
            IRenavigator addStudentRenavigator,
            IRenavigator addEventRenavigator,
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
            NavigateAddStudentCommand = new RenavigateCommand(addStudentRenavigator,ViewDisplayType.Create, viewDisplayTypeService);
            NavigateAddEventCommand = new RenavigateCommand(addEventRenavigator, ViewDisplayType.Create, viewDisplayTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            CreateGroupCommandAsync = new ActionCommandAsync(OnCreateGroupCommandExecutedAsync, CanCreateUpdateGroupCommandExecute);
            UpdateGroupNameCommandAsync = new ActionCommandAsync(OnUpdateGroupNameCommandExecutedAsync, CanCreateUpdateGroupCommandExecute);
            AssignStudentsToGroupCommandAsync = new ActionCommandAsync(OnAssignStudentsToGroupCommandExecute, CanAssignStudentsToGroupCommandExecute);
            AssignEventsToGroupCommandAsync = new ActionCommandAsync(OnAssignEventsToGroupCommandExecute, CanAssignEventsToGroupCommandExecute);
        }

        #region Fields and properties

        private bool isStudentEmpty;
        public bool IsStudentsEmpty
        {
            get => isStudentEmpty;
            set => Set(ref isStudentEmpty, value);
        }

        private bool isEventEmpty;
        public bool IsEventsEmpty
        {
            get => isEventEmpty;
            set => Set(ref isEventEmpty, value);
        }

        public ViewDisplayType? CurrentViewDisplayType
        {
            get
            {
                if (viewDisplayTypeService.CurrentViewDisplayType == ViewDisplayType.Edit)
                {
                    GroupNameToCreate = sharedDataStore.SelectedGroup.Name;
                }

                return viewDisplayTypeService.CurrentViewDisplayType;
            }
        }

        public ObservableCollection<StudentViewModel> students = new();
        public ObservableCollection<StudentViewModel> Students
        {
            get => students;
            set => Set(ref students, value);
        }

        public ObservableCollection<EventsAssignViewModel> events = new ();
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

        private string? createUpdateErrorMessage;
        public string? CreateUpdateErrorMessage
        {
            get => createUpdateErrorMessage;
            set => Set(ref createUpdateErrorMessage, value);
        }

        private string? errorMessageStudents;
        public string? ErrorMessageStudents
        {
            get => errorMessageStudents;
            set => Set(ref errorMessageStudents, value);
        }

        private string? errorMessageEvents;
        public string? ErrorMessageEvents
        {
            get => errorMessageEvents;
            set => Set(ref errorMessageEvents, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateGroupCommand { get; }

        public ICommand NavigateGroupSettingsCommand { get; }

        public ICommand NavigateAddStudentCommand { get; }

        public ICommand NavigateAddEventCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p)
        {
            if (CurrentViewDisplayType == ViewDisplayType.AddEvents
                ||
                CurrentViewDisplayType == ViewDisplayType.AddStudents)
            {
                return true;
            }

            return false;
        }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            if (CurrentViewDisplayType == ViewDisplayType.AddEvents)
            {
                await LoadEventsData();
            }

            if (CurrentViewDisplayType == ViewDisplayType.AddStudents)
            {
                await LoadStudentsData();
            }
        }

        #endregion

        #region CreateGroupCommandAsync

        public ICommandAsync CreateGroupCommandAsync { get; }

        private bool CanCreateUpdateGroupCommandExecute(object p)
        {
            if (string.IsNullOrEmpty(GroupNameToCreate))
            {
                CreateUpdateErrorMessage = "Название группы не может быть пустым";
                return false;
            }

            CreateUpdateErrorMessage = null;
            return true;
        }

        private async Task OnCreateGroupCommandExecutedAsync(object p)
        {
            await groupsService.CreateGroupAsync(GroupNameToCreate, sharedDataStore.CurrentUser.Id);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        #region UpdateGroupNameCommandAsync

        public ICommandAsync UpdateGroupNameCommandAsync { get; }

        private async Task OnUpdateGroupNameCommandExecutedAsync(object p)
        {
            await groupsService.UpdateGroupNameAsync(sharedDataStore.SelectedGroup.Id, GroupNameToCreate);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        #region AssignStudentsToGroupCommandAsync

        public ICommandAsync AssignStudentsToGroupCommandAsync { get; }

        private bool CanAssignStudentsToGroupCommandExecute(object p)
        {
            if (!Students.Where(s => s.IsAssigned).Any())
            {
                ErrorMessageStudents = "Выберите хотя бы одного участника";
                return false;
            }

            ErrorMessageStudents = null;
            return true;
        }

        private async Task OnAssignStudentsToGroupCommandExecute(object p)
        {
            var selectedStudentIds = Students.Where(s => s.IsAssigned).Select(s => s.Id).ToList();

            await groupsService.AssignStudentsToGroupAsync(sharedDataStore.SelectedGroup.Id, selectedStudentIds);

            NavigateGroupSettingsCommand.Execute(p);
        }

        #endregion

        #region AssignEventsToGroupCommandAsync

        public ICommandAsync AssignEventsToGroupCommandAsync { get; }

        private bool CanAssignEventsToGroupCommandExecute(object p)
        {
            if (!Events.Where(s => s.IsAssigned).Any())
            {
                ErrorMessageEvents = "Выберите хотя бы одно событие";
                return false;
            }

            ErrorMessageEvents = null;
            return true;
        }

        private async Task OnAssignEventsToGroupCommandExecute(object p)
        {
            var selectedEventIds = Events.Where(s => s.IsAssigned).Select(s => s.Id).ToList();

            await groupsService.AssignEventsToGroupAsync(sharedDataStore.SelectedGroup.Id, selectedEventIds);

            NavigateGroupSettingsCommand.Execute(p);
        }

        #endregion

        private async Task LoadStudentsData()
        {
            var students = await studentService.GetAllStudentsUnAssignedToGroup<StudentViewModel>(sharedDataStore.SelectedGroup.Id);

            IsStudentsEmpty = !students.Any();

            if (!IsStudentsEmpty)
            {
                Students = new(students);
            }
        }

        private async Task LoadEventsData()
        {
            var events = await eventsService.GetAllEventsFilteredByStatusAndGroupAsync<EventsAssignViewModel>(Status.Ended, sharedDataStore.SelectedGroup.Id, sharedDataStore.CurrentUser.Id);

            IsEventsEmpty = !events.Any();

            if (!IsEventsEmpty) 
            {
                Events = new(events);
            }
        }

        private void ViewDisplayTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewDisplayType));
        }

        public override void Dispose()
        {
            viewDisplayTypeService.StateChanged -= ViewDisplayTypeService_StateChanged;

            base.Dispose();
        }
    }
}
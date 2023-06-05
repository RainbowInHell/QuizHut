namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.GroupViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class GroupSettingsViewModel : ViewModel
    {
        private readonly IGroupsService groupsService;

        private readonly IEventsService eventsService;

        private readonly IStudentsService studentsService;

        private readonly ISharedDataStore sharedDataStore;

        public GroupSettingsViewModel(
            IGroupsService groupsService,
            IEventsService eventsService,
            IStudentsService studentsService,
            ISharedDataStore sharedDataStore,
            IRenavigator addStudentRenavigator,
            IRenavigator addEventsRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            this.groupsService = groupsService;
            this.eventsService = eventsService;
            this.studentsService = studentsService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddStudentsCommand = new RenavigateCommand(addStudentRenavigator, ViewDisplayType.AddStudents, groupSettingsTypeService);
            NavigateAddEventsCommand = new RenavigateCommand(addEventsRenavigator, ViewDisplayType.AddEvents, groupSettingsTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            DeleteStudentFromGroupCommandAsync = new ActionCommandAsync(OnDeleteStudentFromGroupCommandExecutedAsync);
            DeleteEventFromGroupCommandAsync = new ActionCommandAsync(OnDeleteEventFromGroupCommandExecutedAsync);
        }

        #region Fields and properties

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

        private StudentViewModel selectedStudent;
        public StudentViewModel SelectedStudent
        {
            get => selectedStudent;
            set => Set(ref selectedStudent, value);
        }

        private EventsAssignViewModel selectedEvent;
        public EventsAssignViewModel SelectedEvent
        {
            get => selectedEvent;
            set => Set(ref selectedEvent, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddStudentsCommand { get; }

        public ICommand NavigateAddEventsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadEventsData();

            await LoadStudentsData();
        }

        #endregion

        #region DeleteStudentFromGroupCommandAsync

        public ICommandAsync DeleteStudentFromGroupCommandAsync { get; }

        private async Task OnDeleteStudentFromGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteStudentFromGroupAsync(sharedDataStore.SelectedGroup.Id, SelectedStudent.Id);

            await LoadStudentsData();
        }

        #endregion

        #region DeleteEventFromGroupCommandAsync

        public ICommandAsync DeleteEventFromGroupCommandAsync { get; }

        private async Task OnDeleteEventFromGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteEventFromGroupAsync(sharedDataStore.SelectedGroup.Id, SelectedEvent.Id);

            await LoadEventsData();
        }

        #endregion

        private async Task LoadStudentsData()
        {
            var students = await studentsService.GetAllStudentsByGroupIdAsync<StudentViewModel>(sharedDataStore.SelectedGroup.Id);

            Students = new(students);
        }

        private async Task LoadEventsData()
        {
            var events = await eventsService.GetAllEventsByGroupIdAsync<EventsAssignViewModel>(sharedDataStore.SelectedGroup.Id);

            Events = new(events);
        }
    }
}
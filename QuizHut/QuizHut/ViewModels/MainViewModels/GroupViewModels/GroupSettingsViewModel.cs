namespace QuizHut.ViewModels.MainViewModels.GroupViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class GroupSettingsViewModel : ViewModel
    {
        private readonly IGroupsService groupsService;

        private readonly IStudentsService studentService;

        private readonly ISharedDataStore sharedDataStore;

        public GroupSettingsViewModel(
            IGroupsService groupsService,
            IStudentsService studentService,
            ISharedDataStore sharedDataStore,
            IRenavigator addStudentRenavigator, 
            IRenavigator addEventsRenavigator, 
            IViewDisplayTypeService groupSettingsTypeService)
        {
            this.groupsService = groupsService;
            this.studentService = studentService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddStudentsCommand = new RenavigateCommand(addStudentRenavigator, ViewDisplayType.AddStudents, groupSettingsTypeService);
            NavigateAddEventsCommand = new RenavigateCommand(addEventsRenavigator, ViewDisplayType.AddEvents, groupSettingsTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            DeleteStudentFromGroupCommandAsync = new ActionCommandAsync(OnDeleteStudentFromGroupCommandExecutedAsync, CanDeleteStudentFromGroupCommandExecute);
        }

        #region Fields and properties

        public ObservableCollection<StudentViewModel> students;
        public ObservableCollection<StudentViewModel> Students
        {
            get => students;
            set => Set(ref students, value);
        }

        private StudentViewModel selectedStudent;
        public StudentViewModel SelectedStudent
        {
            get => selectedStudent;
            set => Set(ref selectedStudent, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddStudentsCommand { get; }

        public ICommand NavigateAddEventsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadStudentsData();
        }

        #endregion

        #region DeleteStudentFromGroupCommand

        public ICommandAsync DeleteStudentFromGroupCommandAsync { get; }

        private bool CanDeleteStudentFromGroupCommandExecute(object p) => true;

        private async Task OnDeleteStudentFromGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteStudentFromGroupAsync(sharedDataStore.SelectedGroupId, SelectedStudent.Id);

            await LoadStudentsData();
        }

        #endregion

        private async Task LoadStudentsData()
        {
            var students = await studentService.GetAllByGroupIdAsync<StudentViewModel>(sharedDataStore.SelectedGroupId);

            Students = new(students);
        }
    }
}
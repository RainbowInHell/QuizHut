namespace QuizHut.ViewModels.MainViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.ViewModels.Base;

    class StudentsViewModel : ViewModel
    {
        public static string Title { get; } = "Учащиеся";

        public static IconChar IconChar { get; } = IconChar.UserGroup;

        private readonly IStudentService studentService;

        private readonly Dictionary<string, string> SearchCriteriasInEnglish = new Dictionary<string, string>()
        {
            { "Полное имя", "FullName" },
            { "Имя", "FirstName" },
            { "Фамиля", "LastName" },
            { "Почта", "Email" }
        };

        public StudentsViewModel(IStudentService studentService)
        {
            this.studentService = studentService;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            AddStudentToTeacherListCommandAsync = new ActionCommandAsync(OnAddStudentToTeacherListCommandExecute, CanAddStudentToTeacherListCommandExecute);
            RemoveStudentFromTeacherListCommandAsync = new ActionCommandAsync(OnRemoveStudentFromTeacherListCommandExecute, CanRemoveStudentFromTeacherListCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
        }

        #region FieldsAndProperties

        public ObservableCollection<string> SearchCriterias => new(SearchCriteriasInEnglish.Keys);

        private string studentEmailToAdd;
        public string StudentEmailToAdd
        {
            get => studentEmailToAdd;
            set => Set(ref studentEmailToAdd, value);
        }

        private StudentViewModel selectedStudent;
        public StudentViewModel SelectedStudent
        {
            get => selectedStudent;
            set => Set(ref selectedStudent, value);
        }

        public ObservableCollection<StudentViewModel> students;
        public ObservableCollection<StudentViewModel> Students
        {
            get => students;
            set => Set(ref students, value);
        }

        private string searchCriteria;
        public string SearchCriteria
        {
            get => searchCriteria;
            set => Set(ref searchCriteria, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadStudentsData();
        }

        #endregion

        #region AddStudentToTeacherListCommandAsync

        public ICommandAsync AddStudentToTeacherListCommandAsync { get; }

        private bool CanAddStudentToTeacherListCommandExecute(object p) => true;

        private async Task OnAddStudentToTeacherListCommandExecute(object p)
        {
            var partisipantIsAdded = await studentService.AddStudentAsync(StudentEmailToAdd, "aa0f4db3-d1a4-4dbe-a1a5-45313b2f88c3");

            if (partisipantIsAdded)
            {
                await LoadStudentsData();
            }
        }

        #endregion

        #region RemoveStudentFromTeacherListCommandAsync

        public ICommandAsync RemoveStudentFromTeacherListCommandAsync { get; }

        private bool CanRemoveStudentFromTeacherListCommandExecute(object p) => true;

        private async Task OnRemoveStudentFromTeacherListCommandExecute(object p)
        {
            await studentService.DeleteFromTeacherListAsync(SelectedStudent.Id, "aa0f4db3-d1a4-4dbe-a1a5-45313b2f88c3");

            await LoadStudentsData();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => true;

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            var serachCriteria = SearchCriteria is null ? null : SearchCriteriasInEnglish[SearchCriteria];

           await LoadStudentsData(searchCriteria: serachCriteria, searchText: SearchText);
        }

        #endregion

        private async Task LoadStudentsData(string teacherId = "aa0f4db3-d1a4-4dbe-a1a5-45313b2f88c3",
            string groupId = null,
            string searchCriteria = null,
            string searchText = null)
        {
            var students = await studentService.GetAllStudentsAsync<StudentViewModel>(teacherId, groupId, searchCriteria, searchText);

            Students = new(students);
        }
    }
}
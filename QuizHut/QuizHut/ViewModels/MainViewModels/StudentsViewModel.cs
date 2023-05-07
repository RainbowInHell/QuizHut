namespace QuizHut.ViewModels.MainViewModels
{
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

        public ObservableCollection<StudentViewModel> Students { get; } = new ObservableCollection<StudentViewModel>();

        public StudentsViewModel(IStudentService studentService)
        {
            this.studentService = studentService;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
        }

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p) => await LoadData();

        public async Task LoadData()
        {
            Students.Clear();

            var students =  await studentService.GetAllStudentsAsync<StudentViewModel>();

            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
    }
}
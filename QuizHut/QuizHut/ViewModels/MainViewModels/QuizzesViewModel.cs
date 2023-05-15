namespace QuizHut.ViewModels.MainViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class QuizzesViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Викторины";
        public static IconChar IconChar { get; } = IconChar.FolderOpen;

        private readonly IQuizzesService quizzesService;

        private readonly ISharedDataStore sharedDataStore;

        public QuizzesViewModel(IQuizzesService quizzesService, ISharedDataStore sharedDataStore)
        {
            this.quizzesService = quizzesService;
            this.sharedDataStore = sharedDataStore;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
        }

        #region Fields and properties

        public ObservableCollection<QuizListViewModel> quizzes;
        public ObservableCollection<QuizListViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        private QuizListViewModel selectedQuiz;
        public QuizListViewModel SelectedQuiz
        {
            get => selectedQuiz;
            set => Set(ref selectedQuiz, value);
        }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();
        }

        #endregion

        private async Task LoadQuizzesData(string searchCriteria = null, string searchText = null)
        {
            var quizzes = await quizzesService.GetAllQuizzesAsync<QuizListViewModel>(AccountStore.CurrentAdminId, searchCriteria, searchText);

            Quizzes = new(quizzes);
        }
    }
}
namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class CategorySettingsViewModel : ViewModel
    {
        private readonly ICategoriesService categoriesService;

        private readonly IQuizzesService quizzesService;

        private readonly ISharedDataStore sharedDataStore;

        public CategorySettingsViewModel(
            ICategoriesService categoriesService,
            IQuizzesService quizzesService,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizzesRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            this.categoriesService = categoriesService;
            this.quizzesService = quizzesService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuizzesCommand = new RenavigateCommand(addQuizzesRenavigator, ViewDisplayType.AddQuizzes, groupSettingsTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            DeleteQuizFromCategoryCommandAsync = new ActionCommandAsync(OnDeleteQuizFromCategoryCommandExecutedAsync, CanDeleteQuizFromCategoryCommandExecute);
        }

        #region Fields and properties

        public ObservableCollection<QuizAssignViewModel> quizzes;
        public ObservableCollection<QuizAssignViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        private QuizAssignViewModel selectedQuiz;
        public QuizAssignViewModel SelectedQuiz
        {
            get => selectedQuiz;
            set => Set(ref selectedQuiz, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddQuizzesCommand { get; }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();
        }

        #endregion

        #region DeleteQuizFromCategoryCommandAsync

        public ICommandAsync DeleteQuizFromCategoryCommandAsync { get; }

        private bool CanDeleteQuizFromCategoryCommandExecute(object p) => true;

        private async Task OnDeleteQuizFromCategoryCommandExecutedAsync(object p)
        {
            await categoriesService.DeleteQuizFromCategoryAsync(sharedDataStore.SelectedCategory.Id, SelectedQuiz.Id);

            await LoadQuizzesData();
        }

        #endregion

        private async Task LoadQuizzesData()
        {
            var quizzes = await quizzesService.GetQuizzesByCategoryIdAsync<QuizAssignViewModel>(sharedDataStore.SelectedCategory.Id);

            Quizzes = new(quizzes);
        }
    }
}
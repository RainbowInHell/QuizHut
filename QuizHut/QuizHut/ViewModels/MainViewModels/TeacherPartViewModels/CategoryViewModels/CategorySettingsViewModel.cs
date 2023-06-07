namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.CategoryViewModels
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
            IRenavigator quizSettingRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            this.categoriesService = categoriesService;
            this.quizzesService = quizzesService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuizzesCommand = new RenavigateCommand(addQuizzesRenavigator, ViewDisplayType.AddQuizzes, groupSettingsTypeService);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            DeleteQuizFromCategoryCommandAsync = new ActionCommandAsync(OnDeleteQuizFromCategoryCommandExecutedAsync);
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
            get
            {
                sharedDataStore.SelectedQuiz = new() { Id = selectedQuiz?.Id };
                return selectedQuiz;
            }
            set => Set(ref selectedQuiz, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddQuizzesCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();
        }

        #endregion

        #region DeleteQuizFromCategoryCommandAsync

        public ICommandAsync DeleteQuizFromCategoryCommandAsync { get; }

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
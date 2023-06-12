namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class QuizzesViewModel : ViewModel, IMenuView
    {
        public string Title { get; set; } = "Викторины";

        public IconChar IconChar { get; set; } = IconChar.FolderOpen;

        public Dictionary<string, string> SearchCriteriasInEnglish => new()
        {
            { "Название", "Name" },
            { "Назначена", "Assigned" },
            { "Не назначена", "Unassigned" }
        };

        private readonly IQuizzesService quizzesService;

        private readonly ICategoriesService categoriesService;

        private readonly IEventsService eventsService;

        private readonly IDateTimeConverter dateTimeConverter;

        private readonly IExporter exporter;

        private readonly ISharedDataStore sharedDataStore;

        public QuizzesViewModel(
            IQuizzesService quizzesService,
            ICategoriesService categoriesService,
            IEventsService eventsService,
            IDateTimeConverter dateTimeConverter,
            IExporter exporter,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizRenavigator,
            IRenavigator addQuestionRenavigator,
            IRenavigator editQuizRenavigator,
            IRenavigator quizSettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.quizzesService = quizzesService;
            this.categoriesService = categoriesService;
            this.eventsService = eventsService;
            this.dateTimeConverter = dateTimeConverter;
            this.exporter = exporter;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuizCommand = new RenavigateCommand(addQuizRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateAddQuestionCommand = new RenavigateCommand(addQuestionRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditQuizCommand = new RenavigateCommand(editQuizRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
            RefreshSearchCommandAsync = new ActionCommandAsync(OnRefreshSearchCommandAsyncExecute);
            FilterByCategoryCommandAsync = new ActionCommandAsync(OnFilterByCategoryCommandAsyncExecute, CanFilterByCategoryCommandAsyncExecute);
            DeleteQuizCommandAsync = new ActionCommandAsync(OnDeleteQuizCommandExecutedAsync);
            ExportDataAsyncCommand = new ActionCommandAsync(OnExportDataAsyncCommandExecute);
        }

        #region Fields and properties

        public ObservableCollection<QuizListViewModel> quizzes;
        public ObservableCollection<QuizListViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        public ObservableCollection<CategorySimpleViewModel> categories;
        public ObservableCollection<CategorySimpleViewModel> Categories
        {
            get => categories;
            set => Set(ref categories, value);
        }

        private QuizListViewModel selectedQuiz;
        public QuizListViewModel SelectedQuiz
        {
            get
            {
                sharedDataStore.SelectedQuiz = selectedQuiz;
                return selectedQuiz;
            }
            set => Set(ref selectedQuiz, value);
        }

        private CategorySimpleViewModel selectedCategory;
        public CategorySimpleViewModel SelectedCategory
        {
            get => selectedCategory;
            set => Set(ref selectedCategory, value);
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

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddQuizCommand { get; }

        public ICommand NavigateAddQuestionCommand { get; }

        public ICommand NavigateEditQuizCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();

            await LoadCategoriesData();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => !string.IsNullOrEmpty(SearchCriteria) && !string.IsNullOrEmpty(SearchText);

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadQuizzesData(SearchCriteriasInEnglish[SearchCriteria], SearchText);
        }

        #endregion

        #region RefreshSearchCommandAsync

        public ICommandAsync RefreshSearchCommandAsync { get; }

        private async Task OnRefreshSearchCommandAsyncExecute(object p)
        {
            SearchCriteria = null;
            SearchText = null;
            SelectedCategory = null;

            await LoadQuizzesData();
        }

        #endregion

        #region FilterByCategoryCommandAsync

        public ICommandAsync FilterByCategoryCommandAsync { get; }

        private bool CanFilterByCategoryCommandAsyncExecute(object p) => SelectedCategory != null;

        private async Task OnFilterByCategoryCommandAsyncExecute(object p)
        {
            var searchCriteria = SearchCriteria == null ? null : SearchCriteriasInEnglish[SearchCriteria];

            await LoadQuizzesData(searchCriteria, SearchText, SelectedCategory.Id);
        }

        #endregion

        #region DeleteQuizCommandAsync

        public ICommandAsync DeleteQuizCommandAsync { get; }

        private async Task OnDeleteQuizCommandExecutedAsync(object p)
        {
            if (SelectedQuiz.EventId != null)
            {
                await eventsService.DeleteQuizFromEventAsync(SelectedQuiz.EventId, SelectedQuiz.Id);
            }

            await quizzesService.DeleteQuizAsync(SelectedQuiz.Id);

            await LoadQuizzesData();
        }

        #endregion

        #region ExportDataCommand

        public ICommandAsync ExportDataAsyncCommand { get; }

        private async Task OnExportDataAsyncCommandExecute(object p)
        {
            await exporter.GenerateExcelReportAsync(Quizzes);
        }

        #endregion

        private async Task LoadQuizzesData(string searchCriteria = null, string searchText = null, string categoryId = null)
        {
            var quizzes = await quizzesService.GetAllQuizzesAsync<QuizListViewModel>(sharedDataStore.CurrentUser.Id, searchCriteria, searchText, categoryId);

            if (!quizzes.Any())
            {
                ErrorMessage = "Викторины не найдены";
            }
            else
            {
                foreach (var quizz in quizzes)
                {
                    quizz.CreatedOnDate = dateTimeConverter.GetDate(quizz.CreatedOn);
                }

                ErrorMessage = null;
            }

            Quizzes = new(quizzes);
        }

        private async Task LoadCategoriesData()
        {
            var categories = await categoriesService.GetAllCategories<CategorySimpleViewModel>(sharedDataStore.CurrentUser.Id);

            Categories = new(categories);
        }
    }
}
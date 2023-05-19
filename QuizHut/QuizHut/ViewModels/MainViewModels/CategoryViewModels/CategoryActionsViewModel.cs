namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    
    class CategoryActionsViewModel : ViewModel
    {
        private readonly ICategoriesService categoriesService;

        private readonly IQuizzesService quizzesService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public CategoryActionsViewModel(
            ICategoriesService categoriesService,
            IQuizzesService quizzesService,
            ISharedDataStore sharedDataStore,
            IRenavigator categoryRenavigator,
            IRenavigator categorySettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.categoriesService = categoriesService;
            this.quizzesService = quizzesService;
            this.sharedDataStore = sharedDataStore;
            this.viewDisplayTypeService = viewDisplayTypeService;

            viewDisplayTypeService.StateChanged += ViewDisplayTypeService_StateChanged;

            NavigateCategoryCommand = new RenavigateCommand(categoryRenavigator);
            NavigateCategorySettingsCommand = new RenavigateCommand(categorySettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            CreateCategoryCommandAsync = new ActionCommandAsync(OnCreateCategoryCommandExecutedAsync, CanCreateCategoryCommandExecute);
            UpdateCategoryNameCommandAsync = new ActionCommandAsync(OnUpdateCategoryNameCommandExecutedAsync, CanUpdateCategoryNameCommandExecute);
            AssignQuizzesToCategoryCommandAsync = new ActionCommandAsync(OnAssignQuizzesToCategoryCommandExecute, CanAssignQuizzesToCategoryCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType => viewDisplayTypeService.CurrentViewDisplayType;

        private string categoryNameToCreate;
        public string CategoryNameToCreate
        {
            get => categoryNameToCreate;
            set => Set(ref categoryNameToCreate, value);
        }

        public ObservableCollection<QuizAssignViewModel> quizzes;
        public ObservableCollection<QuizAssignViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateCategoryCommand { get; }

        public ICommand NavigateCategorySettingsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();
        }

        #endregion

        #region CreateCategoryCommandAsync

        public ICommandAsync CreateCategoryCommandAsync { get; }

        private bool CanCreateCategoryCommandExecute(object p) => !string.IsNullOrEmpty(CategoryNameToCreate);

        private async Task OnCreateCategoryCommandExecutedAsync(object p)
        {
            await categoriesService.CreateCategoryAsync(CategoryNameToCreate, AccountStore.CurrentAdminId);

            NavigateCategoryCommand.Execute(p);
        }

        #endregion

        #region UpdateCategoryNameCommandAsync

        public ICommandAsync UpdateCategoryNameCommandAsync { get; }

        private bool CanUpdateCategoryNameCommandExecute(object p) => !string.IsNullOrEmpty(CategoryNameToCreate);

        private async Task OnUpdateCategoryNameCommandExecutedAsync(object p)
        {
            await categoriesService.UpdateNameAsync(sharedDataStore.SelectedCategoryId, CategoryNameToCreate);

            NavigateCategoryCommand.Execute(p);
        }

        #endregion

        #region AssignQuizzesToCategoryCommandAsync

        public ICommandAsync AssignQuizzesToCategoryCommandAsync { get; }

        private bool CanAssignQuizzesToCategoryCommandExecute(object p) => true;

        private async Task OnAssignQuizzesToCategoryCommandExecute(object p)
        {
            var selectedQuizIds = Quizzes.Where(s => s.IsAssigned).Select(s => s.Id).ToList();

            if (selectedQuizIds.Any())
            {
                await categoriesService.AssignQuizzesToCategoryAsync(sharedDataStore.SelectedCategoryId, selectedQuizIds);
            }

            NavigateCategorySettingsCommand.Execute(p);
        }

        #endregion

        private async Task LoadQuizzesData()
        {
            var quizzes = await quizzesService.GetUnAssignedToCategoryByCreatorIdAsync<QuizAssignViewModel>(sharedDataStore.SelectedCategoryId, AccountStore.CurrentAdminId);

            Quizzes = new(quizzes);
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
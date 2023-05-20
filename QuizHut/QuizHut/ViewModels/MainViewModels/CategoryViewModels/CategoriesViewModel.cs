namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class CategoriesViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Категории";
        
        public static IconChar IconChar { get; } = IconChar.LayerGroup;

        private readonly ICategoriesService categoriesService;

        private readonly IDateTimeConverter dateTimeConverter;

        private readonly ISharedDataStore sharedDataStore;

        public CategoriesViewModel(
            ICategoriesService categoriesService,
            IDateTimeConverter dateTimeConverter,
            ISharedDataStore sharedDataStore,
            IRenavigator categoryActionsRenavigator,
            IRenavigator categorySettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService) 
        {
            this.categoriesService = categoriesService;
            this.dateTimeConverter = dateTimeConverter;
            this.sharedDataStore = sharedDataStore;

            NavigateCreateCategoryCommand = new RenavigateCommand(categoryActionsRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditCategoryCommand = new RenavigateCommand(categoryActionsRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateCategorySettingsCommand = new RenavigateCommand(categorySettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
            DeleteCategoryCommandAsync = new ActionCommandAsync(OnDeleteCategoryCommandExecutedAsync, CanDeleteCategoryCommandExecute);
        }

        #region FieldsAndProperties

        public ObservableCollection<CategoryViewModel> categories;
        public ObservableCollection<CategoryViewModel> Categories
        {
            get => categories;
            set => Set(ref categories, value);
        }

        private CategoryViewModel selectedCategory;
        public CategoryViewModel SelectedCategory
        {
            get
            {
                sharedDataStore.SelectedCategory = selectedCategory;
                return selectedCategory;
            }
            set => Set(ref selectedCategory, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateCreateCategoryCommand { get; }

        public ICommand NavigateEditCategoryCommand { get; }

        public ICommand NavigateCategorySettingsCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadCategoriesData();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => true;

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadCategoriesData(SearchText);
        }

        #endregion

        #region DeleteCategoryCommandAsync

        public ICommandAsync DeleteCategoryCommandAsync { get; }

        private bool CanDeleteCategoryCommandExecute(object p) => true;

        private async Task OnDeleteCategoryCommandExecutedAsync(object p)
        {
            await categoriesService.DeleteAsync(SelectedCategory.Id);

            await LoadCategoriesData();
        }

        #endregion

        private async Task LoadCategoriesData(string searchText = null)
        {
            var categories = await categoriesService.GetAllCategories<CategoryViewModel>(AccountStore.CurrentAdminId, searchText);

            foreach (var category in categories)
            {
                category.CreatedOnDate = dateTimeConverter.GetDate(category.CreatedOn);
            }

            Categories = new(categories);
        }
    }
}
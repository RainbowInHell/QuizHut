namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{

    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class CategoriesViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Категории";
        public static IconChar IconChar { get; } = IconChar.LayerGroup;

        public CategoriesViewModel(
            IRenavigator categoryActionsRenavigator,
            IRenavigator categorySettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService) 
        {
            NavigateCreateCategoryCommand = new RenavigateCommand(categoryActionsRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditCategoryCommand = new RenavigateCommand(categoryActionsRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateCategorySettingsCommand = new RenavigateCommand(categorySettingRenavigator);
        }

        #region NavigationCommands

        public ICommand NavigateCreateCategoryCommand { get; }

        public ICommand NavigateEditCategoryCommand { get; }

        public ICommand NavigateCategorySettingsCommand { get; }

        #endregion
    }
}
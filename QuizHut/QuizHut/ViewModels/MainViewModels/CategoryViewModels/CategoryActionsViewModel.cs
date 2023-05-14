namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    

    class CategoryActionsViewModel : ViewModel
    {
        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public CategoryActionsViewModel(
            IRenavigator categoryRenavigator,
            IRenavigator categorySettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.viewDisplayTypeService = viewDisplayTypeService;

            viewDisplayTypeService.StateChanged += ViewDisplayTypeService_StateChanged;

            NavigateCategoryCommand = new RenavigateCommand(categoryRenavigator);
            NavigateCategorySettingsCommand = new RenavigateCommand(categorySettingRenavigator);
        }

        #region Fields and properties

        public ViewDisplayType? ViewDisplayType => viewDisplayTypeService.ViewDisplayType;

        #endregion

        #region NavigationCommands

        public ICommand NavigateCategoryCommand { get; }

        public ICommand NavigateCategorySettingsCommand { get; }

        #endregion

        private void ViewDisplayTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(ViewDisplayType));
        }

        public override void Dispose()
        {
            viewDisplayTypeService.StateChanged -= ViewDisplayTypeService_StateChanged;

            base.Dispose();
        }
    }
}
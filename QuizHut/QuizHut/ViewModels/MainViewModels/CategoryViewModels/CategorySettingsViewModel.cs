namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class CategorySettingsViewModel : ViewModel
    {
        public CategorySettingsViewModel(
            IRenavigator addQuizzesRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            NavigateAddQuizzesCommand = new RenavigateCommand(addQuizzesRenavigator, ViewDisplayType.AddQuizzes, groupSettingsTypeService);
        }

        #region NavigationCommands

        public ICommand NavigateAddQuizzesCommand { get; }

        #endregion
    }
}
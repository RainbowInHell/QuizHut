namespace QuizHut.ViewModels.MainViewModels.EventViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class EventSettingsViewModel : ViewModel
    {
        public EventSettingsViewModel(
            IRenavigator addQuizzesRenavigator,
            IRenavigator addGroupsRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            NavigateAddQuizzesCommand = new RenavigateCommand(addQuizzesRenavigator, ViewDisplayType.AddQuizzes, groupSettingsTypeService);
            NavigateAddGroupsCommand = new RenavigateCommand(addGroupsRenavigator, ViewDisplayType.AddGroups, groupSettingsTypeService);
        }

        #region NavigationCommands

        public ICommand NavigateAddQuizzesCommand { get; }

        public ICommand NavigateAddGroupsCommand { get; }

        #endregion
    }
}
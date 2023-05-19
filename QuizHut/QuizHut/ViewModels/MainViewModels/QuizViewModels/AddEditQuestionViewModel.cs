namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class AddEditQuestionViewModel : ViewModel
    {
        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public AddEditQuestionViewModel(
            IRenavigator quizRenavigator,
            IRenavigator quizSettingsRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateQuizCommand = new RenavigateCommand(quizRenavigator);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingsRenavigator);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType => viewDisplayTypeService.CurrentViewDisplayType;

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        #endregion
    }
}

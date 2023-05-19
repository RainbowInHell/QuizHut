namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class AddEditAnswerViewModel : ViewModel
    {
        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public AddEditAnswerViewModel(
            IRenavigator quizSettingsRenavigator,
            IRenavigator newQuestionRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateNewQuestionCommand = new RenavigateCommand(newQuestionRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingsRenavigator);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType => viewDisplayTypeService.CurrentViewDisplayType;

        #endregion

        #region NavigationCommands

        public ICommand NavigateNewQuestionCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        #endregion
    }
}
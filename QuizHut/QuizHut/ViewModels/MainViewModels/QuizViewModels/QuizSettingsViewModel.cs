namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class QuizSettingsViewModel : ViewModel
    {
        public QuizSettingsViewModel(
            IRenavigator addQuestionRenavigator,
            IRenavigator addAnswerRenavigator,
            IRenavigator editQuestionRenavigator,
            IRenavigator editAnswerRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            NavigateAddQuestionCommand = new RenavigateCommand(addQuestionRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateAddAnswerCommand = new RenavigateCommand(addAnswerRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditQuestionCommand = new RenavigateCommand(editQuestionRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateEditAnswerCommand = new RenavigateCommand(editAnswerRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
        }

        #region NavigationCommands

        public ICommand NavigateAddQuestionCommand { get; }

        public ICommand NavigateAddAnswerCommand { get; }

        public ICommand NavigateEditQuestionCommand { get; }

        public ICommand NavigateEditAnswerCommand { get; }

        #endregion
    }
}
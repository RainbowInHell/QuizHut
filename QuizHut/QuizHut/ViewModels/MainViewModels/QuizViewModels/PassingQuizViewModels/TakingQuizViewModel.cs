namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class TakingQuizViewModel : ViewModel
    {
        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator nextQuestionRenavigator;

        public TakingQuizViewModel(
            ISharedDataStore sharedDataStore,
            IRenavigator nextQuestionRenavigator,
            IRenavigator endQuizRenavigator)
        {
            this.sharedDataStore = sharedDataStore;
            this.nextQuestionRenavigator = nextQuestionRenavigator;

            Questions = new(this.sharedDataStore.QuizToPass.Questions);
            CurrentQuestion = sharedDataStore.CurrentQuestion is null ? Questions.First() : sharedDataStore.CurrentQuestion;

            GoToNextQuestionCommand = new ActionCommand(OnGoToNextQuestionCommandExecuted, CanGoToNextQuestionCommandExecute);
            GoToPreviousQuestionCommand = new ActionCommand(OnGoToPreviousQuestionCommandExecuted, CanGoToPreviousQuestionCommandExecute);

            NavigateEndQuizCommand = new RenavigateCommand(endQuizRenavigator);
        }

        #region Fields and properties

        private ObservableCollection<AttemtedQuizQuestionViewModel> questions;
        public ObservableCollection<AttemtedQuizQuestionViewModel> Questions
        {
            get => questions;
            set => Set(ref questions, value);
        }

        private AttemtedQuizQuestionViewModel currentQuestion;
        public AttemtedQuizQuestionViewModel CurrentQuestion
        {
            get => currentQuestion;
            set => Set(ref currentQuestion, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateEndQuizCommand { get; }

        #endregion

        #region GoToNextQuestionCommand

        public ICommand GoToNextQuestionCommand { get; }

        private bool CanGoToNextQuestionCommandExecute(object p)
        {
            return Questions.IndexOf(CurrentQuestion) < Questions.Count - 1;
        }

        private void OnGoToNextQuestionCommandExecuted(object p)
        {
            var currentIndex = Questions.IndexOf(CurrentQuestion);
            sharedDataStore.CurrentQuestion = Questions[currentIndex + 1];

            nextQuestionRenavigator.Renavigate();
        }

        #endregion

        #region GoToPreviousQuestionCommand

        public ICommand GoToPreviousQuestionCommand { get; }

        private bool CanGoToPreviousQuestionCommandExecute(object p)
        {
            return Questions.IndexOf(CurrentQuestion) > 0;
        }

        private void OnGoToPreviousQuestionCommandExecuted(object p)
        {
            var currentIndex = Questions.IndexOf(CurrentQuestion);
            sharedDataStore.CurrentQuestion = Questions[currentIndex - 1];

            nextQuestionRenavigator.Renavigate();
        }

        #endregion
    }
}
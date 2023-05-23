namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class StartQuizViewModel : ViewModel
    {
        private readonly ISharedDataStore sharedDataStore;

        public StartQuizViewModel(
            ISharedDataStore sharedDataStore,
            IRenavigator takingQuizRenavigator,
            IRenavigator homeRenavigator)
        {
            this.sharedDataStore = sharedDataStore;

            NavigateTakingQuizCOmmand = new RenavigateCommand(takingQuizRenavigator);
            NavigateHomeCommand = new RenavigateCommand(homeRenavigator);
        }

        #region Fields and properties

        private AttemtedQuizViewModel currentQuiz;
        public AttemtedQuizViewModel CurrentQuiz
        {
            get
            {
                currentQuiz = sharedDataStore.QuizToPass;
                return currentQuiz;
            }
            set => Set(ref currentQuiz, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateTakingQuizCOmmand { get; }

        public ICommand NavigateHomeCommand { get; }

        #endregion
    }
}
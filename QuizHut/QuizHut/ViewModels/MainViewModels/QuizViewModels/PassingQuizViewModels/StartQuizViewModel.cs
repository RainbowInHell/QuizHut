namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System;
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class StartQuizViewModel : ViewModel
    {
        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator takingQuizRenavigator;

        public StartQuizViewModel(
            ISharedDataStore sharedDataStore,
            IRenavigator takingQuizRenavigator,
            IRenavigator homeRenavigator)
        {
            this.sharedDataStore = sharedDataStore;
            this.takingQuizRenavigator = takingQuizRenavigator;

            NavigateTakingQuizCommand = new ActionCommand(p => OnNavigateTakingQuizCommandExecute());
            NavigateHomeCommand = new RenavigateCommand(homeRenavigator);
        }

        #region Fields and properties

        private AttemptedQuizViewModel currentQuiz;
        public AttemptedQuizViewModel CurrentQuiz
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

        public ICommand NavigateTakingQuizCommand { get; }
        
        private void OnNavigateTakingQuizCommandExecute()
        {
            sharedDataStore.RemainingTime = TimeSpan.FromMinutes(CurrentQuiz.Timer);
            
            takingQuizRenavigator.Renavigate();
        }

        public ICommand NavigateHomeCommand { get; }

        #endregion
    }
}
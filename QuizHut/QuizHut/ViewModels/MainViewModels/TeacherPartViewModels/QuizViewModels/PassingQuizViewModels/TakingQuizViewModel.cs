namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels.PassingQuizViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Threading;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class TakingQuizViewModel : ViewModel
    {
        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator nextQuestionRenavigator;

        private readonly IRenavigator endQuizRenavigator;

        public TakingQuizViewModel(
            ISharedDataStore sharedDataStore,
            IRenavigator nextQuestionRenavigator,
            IRenavigator endQuizRenavigator)
        {
            this.sharedDataStore = sharedDataStore;

            this.nextQuestionRenavigator = nextQuestionRenavigator;
            this.endQuizRenavigator = endQuizRenavigator;

            Questions = new(sharedDataStore.QuizToPass.Questions);
            CurrentQuestion = sharedDataStore.CurrentQuestion ?? Questions.First();

            NavigateEndQuizCommand = new RenavigateCommand(endQuizRenavigator);

            GoToNextQuestionCommand = new ActionCommand(OnGoToNextQuestionCommandExecuted, CanGoToNextQuestionCommandExecute);
            GoToPreviousQuestionCommand = new ActionCommand(OnGoToPreviousQuestionCommandExecuted, CanGoToPreviousQuestionCommandExecute);
            StopTimerAndGoToEndQuizCommand = new ActionCommand(OnStopTimerAndGoToEndQuizCommandExecuted);

            SetupTimer();
        }

        #region Fields and properties

        private DispatcherTimer timer;
        public DispatcherTimer Timer
        {
            get => timer;
            set => Set(ref timer, value);
        }

        private ObservableCollection<AttemptedQuizQuestionViewModel> questions;
        public ObservableCollection<AttemptedQuizQuestionViewModel> Questions
        {
            get => questions;
            set => Set(ref questions, value);
        }

        private AttemptedQuizQuestionViewModel currentQuestion;
        public AttemptedQuizQuestionViewModel CurrentQuestion
        {
            get => currentQuestion;
            set => Set(ref currentQuestion, value);
        }

        private TimeSpan timeRemaining;
        public string TimeRemainingText => timeRemaining.ToString(@"hh\:mm\:ss");

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

            sharedDataStore.RemainingTime = timeRemaining;

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

            sharedDataStore.RemainingTime = timeRemaining;

            nextQuestionRenavigator.Renavigate();
        }

        #endregion

        #region StopTimerAndGoToEndQuizCommand

        public ICommand StopTimerAndGoToEndQuizCommand { get; }

        private void OnStopTimerAndGoToEndQuizCommandExecuted(object p)
        {
            EndQuiz();
        }

        #endregion

        #region TimerCommand

        private void TimerTick(object sender, EventArgs e)
        {
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            if (timeRemaining <= TimeSpan.Zero)
            {
                EndQuiz();
            }
            else
            {
                OnPropertyChanged(nameof(TimeRemainingText));
            }
        }

        private void StartTimer()
        {
            timeRemaining = sharedDataStore.RemainingTime;

            Timer.Start();
        }

        private void EndQuiz()
        {
            Timer.Stop();

            sharedDataStore.RemainingTime = TimeSpan.FromMinutes(sharedDataStore.QuizToPass.Timer) - timeRemaining;

            endQuizRenavigator.Renavigate();
        }

        private void SetupTimer()
        {
            if (sharedDataStore.DispatcherTimer == null)
            {
                sharedDataStore.DispatcherTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
            }

            Timer = sharedDataStore.DispatcherTimer;
            Timer.Tick += TimerTick;

            if (sharedDataStore.RemainingTime > TimeSpan.Zero)
            {
                StartTimer();
            }
        }

        #endregion
    }
}
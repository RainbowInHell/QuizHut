namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System;
    using System.Windows.Input;
    using System.Windows.Threading;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.ViewModels.Base;

    class CountDownViewModel : ViewModel
    {
        private DispatcherTimer timer;
        private TimeSpan timeRemaining;
        public string TimeRemainingText => timeRemaining.ToString(@"mm\:ss");

        public CountDownViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;

            StartQuizTimerCommand = new ActionCommand(p => StartQuizTimer());
            StartQuizTimerCommand.Execute(this);
        }

        public ICommand NavigationCommand { get; }
        public ICommand StartQuizTimerCommand { get; }

        private void TimerTick(object sender, EventArgs e)
        {
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            if (timeRemaining <= TimeSpan.Zero)
            {
                // Timer has reached zero, handle timer completion here
                timer.Stop();
                // Additional logic to handle quiz completion when the timer runs out
            }
            else
            {
                // Timer is still running, update the time remaining text
                OnPropertyChanged(nameof(TimeRemainingText));
            }
        }

        private void StartQuizTimer()
        {
            timeRemaining = TimeSpan.FromMinutes(10); // Set the initial quiz duration here
            timer.Start();
        }
    }
}
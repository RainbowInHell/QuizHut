namespace QuizHut.ViewModels.MainViewModels.ResultViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResultsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Результаты"; 
        public static IconChar IconChar { get; } = IconChar.Award;

        public ResultsViewModel(
            IRenavigator activeEventsRenavigator,
            IRenavigator endedEventsRenavigator,
            IRenavigator resultsForEventRenavigator)
        {
            NavigateActiveEventsCommand = new RenavigateCommand(activeEventsRenavigator);
            NavigateEndedEventsCommand = new RenavigateCommand(endedEventsRenavigator);
            NavigateResultsForEventCommand = new RenavigateCommand(resultsForEventRenavigator);
        }

        #region NavigationCommands

        public ICommand NavigateActiveEventsCommand { get; }

        public ICommand NavigateEndedEventsCommand { get; }

        public ICommand NavigateResultsForEventCommand { get; }

        #endregion
    }
}
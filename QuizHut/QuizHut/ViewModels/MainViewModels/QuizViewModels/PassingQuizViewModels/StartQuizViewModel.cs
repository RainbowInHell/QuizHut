namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class StartQuizViewModel : ViewModel
    {
        public StartQuizViewModel(
            IRenavigator takingQuizRenavigator,
            IRenavigator homeRenavigator)
        {
            NavigateTakingQuizCOmmand = new RenavigateCommand(takingQuizRenavigator);
            NavigateHomeCommand = new RenavigateCommand(homeRenavigator);
        }

        #region NavigationCommands

        public ICommand NavigateTakingQuizCOmmand { get; }

        public ICommand NavigateHomeCommand { get; }

        #endregion
    }
}
namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class EndQuizViewModel : ViewModel
    {
        public EndQuizViewModel(
            IRenavigator homeRenavigator)
        {
            NavigateHomeCommand = new RenavigateCommand(homeRenavigator);
        }

        #region NavigationCommands

        public ICommand NavigateHomeCommand { get; }

        #endregion
    }
}
namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class TakingQuizViewModel : ViewModel
    {
        public TakingQuizViewModel(
            IRenavigator endQuizRenavigator)
        {
            NavigateEndQuizCommand = new RenavigateCommand(endQuizRenavigator);
        }

        #region NavigationCommands

        public ICommand NavigateEndQuizCommand { get; }

        #endregion
    }
}
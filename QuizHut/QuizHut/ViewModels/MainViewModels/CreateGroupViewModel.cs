namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class CreateGroupViewModel : ViewModel
    {
        public CreateGroupViewModel(IRenavigator groupRenavigator) 
        { 
            NavigateGroupCommand = new RenavigateCommand(groupRenavigator);
        }

        #region Commands

        public ICommand NavigateGroupCommand { get; }

        #endregion
    }
}
namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class AddEditQuizViewModel : ViewModel
    {
        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public AddEditQuizViewModel(
            IRenavigator quizRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateQuizCommand = new RenavigateCommand(quizRenavigator);
        }

        #region Fields and properties

        public ViewDisplayType? ViewDisplayType => viewDisplayTypeService.ViewDisplayType;

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizCommand { get; }

        #endregion
    }
}

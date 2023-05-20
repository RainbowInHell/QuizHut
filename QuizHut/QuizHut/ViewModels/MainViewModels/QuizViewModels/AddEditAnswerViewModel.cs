namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class AddEditAnswerViewModel : ViewModel
    {
        private readonly IAnswersService answersService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public AddEditAnswerViewModel(
            IAnswersService answersService,
            ISharedDataStore sharedDataStore,
            IRenavigator newAnswerRenavigator,
            IRenavigator quizSettingsRenavigator,
            IRenavigator newQuestionRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.answersService = answersService;
            this.sharedDataStore = sharedDataStore;
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateNewAnswerCommand = new RenavigateCommand(newAnswerRenavigator, Infrastructure.Services.Contracts.ViewDisplayType.Create, viewDisplayTypeService);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingsRenavigator);
            NavigateNewQuestionCommand = new RenavigateCommand(newQuestionRenavigator, Infrastructure.Services.Contracts.ViewDisplayType.Create, viewDisplayTypeService);
        
            CreateAnswerCommandAsync = new ActionCommandAsync(OnCreateAnswerCommandExecutedAsync, CanCreateAnswerCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType => viewDisplayTypeService.CurrentViewDisplayType;

        private bool isRightAnswer;
        public bool IsRightAnswer
        {
            get => isRightAnswer;
            set => Set(ref isRightAnswer, value);
        }

        private string answerDescriptionToCreate;
        public string AnswerDescriptionToCreate
        {
            get => answerDescriptionToCreate;
            set => Set(ref answerDescriptionToCreate, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizSettingsCommand { get; }

        public ICommand NavigateNewQuestionCommand { get; }

        public ICommand NavigateNewAnswerCommand { get; }

        #endregion

        #region CreateAnswerCommandAsync

        public ICommandAsync CreateAnswerCommandAsync { get; }

        private bool CanCreateAnswerCommandExecute(object p) => true;

        private async Task OnCreateAnswerCommandExecutedAsync(object p)
        {
            await answersService.CreateAnswerAsync(AnswerDescriptionToCreate, IsRightAnswer, sharedDataStore.SelectedQuestionId);

            NavigateNewAnswerCommand.Execute(p);
        }

        #endregion

        //#region UpdateAnswerCommandAsync

        //public ICommandAsync UpdateAnswerCommandAsync { get; }

        //private bool CanUpdateAnswerCommandExecute(object p) => true;

        //private async Task OnUpdateAnswerCommandExecutedAsync(object p)
        //{
        //    await answerService.UpdateAsync(sharedDataStore.SelectedAnswerId, "текст ответа", "правильный ли вопрос");

        //    //навигация
        //}

        //#endregion

        //#region DeleteAnswerCommandAsync

        //public ICommandAsync DeleteAnswerCommandAsync { get; }

        //private bool CanDeleteAnswerCommandExecute(object p) => true;

        //private async Task OnDeleteAnswerCommandExecutedAsync(object p)
        //{
        //    await answerService.DeleteAsync(sharedDataStore.SelectedAnswerId);

        //    //навигация
        //}

        //#endregion
    }
}
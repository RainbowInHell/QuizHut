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

    class AddEditQuestionViewModel : ViewModel
    {
        private readonly IQuestionsService questionsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public AddEditQuestionViewModel(
            IQuestionsService questionsService,
            ISharedDataStore sharedDataStore,
            IRenavigator quizRenavigator,
            IRenavigator quizSettingsRenavigator,
            IRenavigator answerCreateRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.questionsService = questionsService;
            this.sharedDataStore = sharedDataStore;
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateQuizCommand = new RenavigateCommand(quizRenavigator);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingsRenavigator);
            NavigateCreateAnswerCommand = new RenavigateCommand(answerCreateRenavigator, Infrastructure.Services.Contracts.ViewDisplayType.Create, viewDisplayTypeService);

            CreateQuestionCommandAsync = new ActionCommandAsync(OnCreateQuestionCommandExecutedAsync, CanCreateQuestionCommandExecute);
            UpdateQuestionCommandAsync = new ActionCommandAsync(OnUpdateQuestionCommandExecutedAsync, CanUpdateQuestionCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType => viewDisplayTypeService.CurrentViewDisplayType;

        private string questionDescriptionToCreate;
        public string QuestionDescriptionToCreate
        {
            get => questionDescriptionToCreate;
            set => Set(ref questionDescriptionToCreate, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        public ICommand NavigateCreateAnswerCommand { get; }

        #endregion

        #region CreateQuestionCommandAsync

        public ICommandAsync CreateQuestionCommandAsync { get; }

        private bool CanCreateQuestionCommandExecute(object p) => true;
        private async Task OnCreateQuestionCommandExecutedAsync(object p)
        {
            var questionId = await questionsService.CreateQuestionAsync(sharedDataStore.SelectedQuiz.Id, QuestionDescriptionToCreate);

            sharedDataStore.SelectedQuestionId = questionId;

            NavigateCreateAnswerCommand.Execute(p);
        }

        #endregion

        #region UpdateQuestionCommandAsync

        public ICommandAsync UpdateQuestionCommandAsync { get; }

        private bool CanUpdateQuestionCommandExecute(object p) => true;

        private async Task OnUpdateQuestionCommandExecutedAsync(object p)
        {
            await questionsService.Update(sharedDataStore.SelectedQuestionId, QuestionDescriptionToCreate);

            //навигация на шестеренку
        }

        #endregion

        //#region DeleteQuestionCommandAsync

        //public ICommandAsync DeleteQuestionCommandAsync { get; }

        //private bool CanDeleteQuestionCommandExecute(object p) => true;

        //private async Task OnDeleteQuestionCommandExecutedAsync(object p)
        //{
        //    await questionsService.DeleteQuestionByIdAsync(sharedDataStore.SelectedQuestionId);
        //}

        //#endregion
    }
}
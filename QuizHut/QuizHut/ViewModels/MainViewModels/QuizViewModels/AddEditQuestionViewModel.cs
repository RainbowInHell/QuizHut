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
            NavigateCreateAnswerCommand = new RenavigateCommand(answerCreateRenavigator, ViewDisplayType.Create, viewDisplayTypeService);

            CreateQuestionCommandAsync = new ActionCommandAsync(OnCreateQuestionCommandExecutedAsync, CanCreateQuestionCommandExecute);
            UpdateQuestionCommandAsync = new ActionCommandAsync(OnUpdateQuestionCommandExecutedAsync, CanUpdateQuestionCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType
        {
            get
            {
                if (viewDisplayTypeService.CurrentViewDisplayType == ViewDisplayType.Edit)
                {
                    QuestionDescriptionToCreate = sharedDataStore.SelectedQuestion.Text;
                }

                return viewDisplayTypeService.CurrentViewDisplayType;
            }
        }

        private string questionDescriptionToCreate;
        public string QuestionDescriptionToCreate
        {
            get => questionDescriptionToCreate;
            set => Set(ref questionDescriptionToCreate, value);
        }

        private bool isFullEvaluation;
        public bool IsFullEvaluation
        {
            get => isFullEvaluation;
            set => Set(ref isFullEvaluation, value);
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
            var questionId = await questionsService.CreateQuestionAsync(sharedDataStore.SelectedQuiz.Id, IsFullEvaluation, QuestionDescriptionToCreate);

            if (sharedDataStore.SelectedQuestion == null)
            {
                sharedDataStore.SelectedQuestion = new()
                {
                    Id = questionId
                };
            }

            NavigateCreateAnswerCommand.Execute(p);
        }

        #endregion

        #region UpdateQuestionCommandAsync

        public ICommandAsync UpdateQuestionCommandAsync { get; }

        private bool CanUpdateQuestionCommandExecute(object p) => true;

        private async Task OnUpdateQuestionCommandExecutedAsync(object p)
        {
            await questionsService.UpdateQuestionAsync(sharedDataStore.SelectedQuestion.Id, IsFullEvaluation, QuestionDescriptionToCreate);

            NavigateQuizSettingsCommand.Execute(p);
        }

        #endregion
    }
}
namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels
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

            CreateQuestionCommandAsync = new ActionCommandAsync(OnCreateQuestionCommandExecutedAsync, CanCreateUpdateQuestionCommandExecute);
            UpdateQuestionCommandAsync = new ActionCommandAsync(OnUpdateQuestionCommandExecutedAsync, CanCreateUpdateQuestionCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType
        {
            get
            {
                if (viewDisplayTypeService.CurrentViewDisplayType == ViewDisplayType.Edit)
                {
                    QuestionDescriptionToCreate = sharedDataStore.SelectedQuestion.Text;
                    IsFullEvaluation = sharedDataStore.SelectedQuestion.IsFullEvaluation;
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

        private string? createUpdateErrorMessage;
        public string? CreateUpdateErrorMessage
        {
            get => createUpdateErrorMessage;
            set => Set(ref createUpdateErrorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        public ICommand NavigateCreateAnswerCommand { get; }

        #endregion

        #region CreateQuestionCommandAsync

        public ICommandAsync CreateQuestionCommandAsync { get; }

        private bool CanCreateUpdateQuestionCommandExecute(object p)
        {
            if (string.IsNullOrEmpty(QuestionDescriptionToCreate))
            {
                CreateUpdateErrorMessage = "Все поля должны быть заполнены";
                return false;
            }

            CreateUpdateErrorMessage = null;
            return true;
        }

        private async Task OnCreateQuestionCommandExecutedAsync(object p)
        {
            var questionId = await questionsService.CreateQuestionAsync(sharedDataStore.SelectedQuiz.Id, IsFullEvaluation, QuestionDescriptionToCreate);

            sharedDataStore.SelectedQuestion = new() { Id = questionId };

            NavigateCreateAnswerCommand.Execute(p);
        }

        #endregion

        #region UpdateQuestionCommandAsync

        public ICommandAsync UpdateQuestionCommandAsync { get; }

        private async Task OnUpdateQuestionCommandExecutedAsync(object p)
        {
            await questionsService.UpdateQuestionAsync(sharedDataStore.SelectedQuestion.Id, IsFullEvaluation, QuestionDescriptionToCreate);

            NavigateQuizSettingsCommand.Execute(p);
        }

        #endregion
    }
}
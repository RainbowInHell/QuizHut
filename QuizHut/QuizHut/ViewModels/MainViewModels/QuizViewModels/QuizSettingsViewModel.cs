namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Answers;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class QuizSettingsViewModel : ViewModel
    {
        private readonly IQuestionsService questionsService;

        private readonly IAnswersService answersService;

        private readonly ISharedDataStore sharedDataStore;

        public QuizSettingsViewModel(
            IQuestionsService questionsService,
            IAnswersService answersService,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuestionRenavigator,
            IRenavigator addAnswerRenavigator,
            IRenavigator editQuestionRenavigator,
            IRenavigator editAnswerRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.questionsService = questionsService;
            this.answersService = answersService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuestionCommand = new RenavigateCommand(addQuestionRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateAddAnswerCommand = new RenavigateCommand(addAnswerRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditQuestionCommand = new RenavigateCommand(editQuestionRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateEditAnswerCommand = new RenavigateCommand(editAnswerRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            DeleteQuestionCommandAsync = new ActionCommandAsync(OnDeleteQuestionCommandExecutedAsync, CanDeleteQuestionCommandExecute);
            DeleteAnswerCommandAsync = new ActionCommandAsync(OnDeleteAnswerCommandExecutedAsync, CanDeleteAnswerCommandExecute);
        }

        #region Fields and properties

        public ObservableCollection<QuestionViewModel> questions;
        public ObservableCollection<QuestionViewModel> Questions
        {
            get => questions;
            set => Set(ref questions, value);
        }

        private QuestionViewModel selectedQuestion;
        public QuestionViewModel SelectedQuestion
        {
            get
            {
                sharedDataStore.SelectedQuestion = selectedQuestion;
                return selectedQuestion;
            }
            set => Set(ref selectedQuestion, value);
        }

        private AnswerViewModel selectedAnswer;
        public AnswerViewModel SelectedAnswer
        {
            get
            {
                sharedDataStore.SelectedAnswer = selectedAnswer;
                return selectedAnswer;
            }
            set => Set(ref selectedAnswer, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddQuestionCommand { get; }

        public ICommand NavigateAddAnswerCommand { get; }

        public ICommand NavigateEditQuestionCommand { get; }

        public ICommand NavigateEditAnswerCommand { get; }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuestionsData();
        }

        #endregion

        #region DeleteQuestionCommandAsync

        public ICommandAsync DeleteQuestionCommandAsync { get; }

        private bool CanDeleteQuestionCommandExecute(object p) => true;

        private async Task OnDeleteQuestionCommandExecutedAsync(object p)
        {
            await questionsService.DeleteQuestionByIdAsync(SelectedQuestion.Id);

            await LoadQuestionsData();
        }

        #endregion

        #region DeleteAnswerCommandAsync

        public ICommandAsync DeleteAnswerCommandAsync { get; }

        private bool CanDeleteAnswerCommandExecute(object p) => true;

        private async Task OnDeleteAnswerCommandExecutedAsync(object p)
        {
            await answersService.DeleteAnswerAsync(SelectedAnswer.Id);

            await LoadQuestionsData();
        }

        #endregion

        private async Task LoadQuestionsData()
        {
            var questions = await questionsService.GetAllQuestionsByQuizIdAsync<QuestionViewModel>(sharedDataStore.SelectedQuiz.Id);

            Questions = new(questions);
        }
    }
}
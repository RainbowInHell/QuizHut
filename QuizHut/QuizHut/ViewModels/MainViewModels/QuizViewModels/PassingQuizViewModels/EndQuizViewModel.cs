namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class EndQuizViewModel : ViewModel
    {
        private readonly IQuestionsService questionsService;

        private readonly IResultsService resultsService;

        private readonly IResultHelper resultHelper;
        
        private readonly ISharedDataStore sharedDataStore;

        public EndQuizViewModel(
            IQuestionsService questionsService,
            IResultsService resultsService,
            IResultHelper resultHelper,
            ISharedDataStore sharedDataStore,
            IRenavigator homeRenavigator)
        {
            this.questionsService = questionsService;
            this.resultsService = resultsService;
            this.resultHelper = resultHelper;
            this.sharedDataStore = sharedDataStore;

            NavigateHomeCommand = new RenavigateCommand(homeRenavigator);

            CalculateQuizResultCommandAsync = new ActionCommandAsync(OnCalculateQuizResultCommandExecutedAsync, CanCalculateQuizResultCommandExecuteAsync);
        }

        #region Fields and properties

        private AttemptedQuizViewModel currentQuiz;
        public AttemptedQuizViewModel CurrentQuiz
        {
            get => currentQuiz;
            set => Set(ref currentQuiz, value);
        }

        private string resultAsString;
        public string ResultAsString
        {
            get => resultAsString;
            set => Set(ref resultAsString, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateHomeCommand { get; }

        #endregion

        #region CalculateQuizResultCommand

        public ICommandAsync CalculateQuizResultCommandAsync { get; }

        private bool CanCalculateQuizResultCommandExecuteAsync(object p) => true;

        private async Task OnCalculateQuizResultCommandExecutedAsync(object p)
        {
            await CalculateQuizResult();
        }

        #endregion

        private async Task CalculateQuizResult()
        {
            CurrentQuiz = sharedDataStore.QuizToPass;

            var originalQuestions = await questionsService.GetAllQuestionsByQuizIdAsync<QuestionViewModel>(CurrentQuiz.Id);

            var receivedPoints = resultHelper.CalculateResult(originalQuestions, CurrentQuiz.Questions);

            ResultAsString = $"{receivedPoints}/{originalQuestions.Count}";

            await resultsService.UpdateResultAsync(sharedDataStore.CurrentResultId, receivedPoints);

            sharedDataStore.CurrentQuestion = null;
        }
    }
}
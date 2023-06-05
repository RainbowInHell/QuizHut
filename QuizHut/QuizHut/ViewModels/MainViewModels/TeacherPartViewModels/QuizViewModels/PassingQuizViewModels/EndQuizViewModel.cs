namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers.Contracts;
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

        private readonly IAccountStore accountStore;

        private readonly IRenavigator homeRenavigator;

        private readonly IRenavigator studentHomeRenavigator;

        public EndQuizViewModel(
            IQuestionsService questionsService,
            IResultsService resultsService,
            IResultHelper resultHelper,
            ISharedDataStore sharedDataStore,
            IAccountStore accountStore,
            IRenavigator homeRenavigator,
            IRenavigator studentHomeRenavigator)
        {
            this.questionsService = questionsService;
            this.resultsService = resultsService;
            this.resultHelper = resultHelper;
            this.sharedDataStore = sharedDataStore;
            this.accountStore = accountStore;

            this.homeRenavigator = homeRenavigator;
            this.studentHomeRenavigator = studentHomeRenavigator;

            QuitQuizCommand = new ActionCommand(OnQuitQuizCommandExecuted);

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

        private string timePassedText;
        public string TimePassedText
        {
            get => timePassedText;
            set => Set(ref timePassedText, value);
        }

        #endregion

        #region QuitQuizCommand

        public ICommand QuitQuizCommand { get; }

        private void OnQuitQuizCommandExecuted(object p)
        {
            if (accountStore.CurrentUserRole == UserRole.Student)
            {
                studentHomeRenavigator.Renavigate();
            }
            else
            {
                homeRenavigator.Renavigate();
            }
        }

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

            TimePassedText = sharedDataStore.RemainingTime.ToString(@"hh\:mm\:ss");

            if (sharedDataStore.CurrentUserRole == UserRole.Student)
            {
                await resultsService.UpdateResultAsync(sharedDataStore.CurrentResultId, receivedPoints, sharedDataStore.RemainingTime);
            }

            sharedDataStore.CurrentQuestion = null;
        }
    }
}
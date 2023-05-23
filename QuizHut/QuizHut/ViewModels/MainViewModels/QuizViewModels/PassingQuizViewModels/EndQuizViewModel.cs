namespace QuizHut.ViewModels.MainViewModels.QuizViewModels.PassingQuizViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
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

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
        }

        #region Fields and properties

        private ObservableCollection<QuestionViewModel> originalQuestions;
        public ObservableCollection<QuestionViewModel> OriginalQuestions
        {
            get => originalQuestions;
            set => Set(ref originalQuestions, value);
        }

        private ObservableCollection<AttemtedQuizQuestionViewModel> solvedQuestions;
        public ObservableCollection<AttemtedQuizQuestionViewModel> SolvedQuestions
        {
            get => solvedQuestions;
            set => Set(ref solvedQuestions, value);
        }

        private int receivedPoints;
        public int ReceivedPoints
        {
            get => receivedPoints;
            set => Set(ref receivedPoints, value);
        }

        private int maxPoints;
        public int MaxPoints
        {
            get => maxPoints;
            set => Set(ref maxPoints, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateHomeCommand { get; }

        #endregion

        #region LoadDataCommand

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuestionsData();
        }

        #endregion

        private async Task LoadQuestionsData()
        {
            var questions = await questionsService.GetAllQuestionsByQuizIdAsync<QuestionViewModel>(sharedDataStore.QuizToPass.Id);

            OriginalQuestions = new(questions);
            SolvedQuestions = new(sharedDataStore.QuizToPass.Questions);

            MaxPoints = questions.Sum(question => question.Answers.Count(answer => answer.IsRightAnswer));

            ReceivedPoints = resultHelper.CalculateResult(OriginalQuestions, SolvedQuestions);

            await resultsService.CreateResultAsync(AccountStore.CurrentAdminId, ReceivedPoints, OriginalQuestions.Count, sharedDataStore.QuizToPass.Id);
        }
    }
}
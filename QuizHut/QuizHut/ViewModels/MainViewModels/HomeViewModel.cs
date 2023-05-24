namespace QuizHut.ViewModels.MainViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class HomeViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Главная";
     
        public static IconChar IconChar { get; } = IconChar.Home;

        private readonly IQuizzesService quizzesService;

        private readonly IResultsService resultsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IShuffler shuffler;

        private readonly IRenavigator startQuizRenavigator;

        public HomeViewModel(
            IQuizzesService quizzesService,
            IResultsService resultsService,
            IShuffler shuffler,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizRenavigator,
            IRenavigator startQuizRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.quizzesService = quizzesService;
            this.resultsService = resultsService;
            this.shuffler = shuffler;
            this.sharedDataStore = sharedDataStore;
            this.startQuizRenavigator = startQuizRenavigator;

            NavigateAddQuizCommand = new RenavigateCommand(addQuizRenavigator, ViewDisplayType.Create, viewDisplayTypeService);

            GoToStartQuizCommandAsync = new ActionCommandAsync(OnGoToStartQuizExecutedAsync, CanGoToStartQuizExecute);
        }

        #region Fields and properties

        private string quizPassword;
        public string QuizPassword
        {
            get => quizPassword;
            set => Set(ref quizPassword, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateAddQuizCommand { get; }

        #endregion

        #region GoToStartQuizCommandAsync

        public ICommandAsync GoToStartQuizCommandAsync { get; }

        private bool CanGoToStartQuizExecute(object p) => !string.IsNullOrEmpty(QuizPassword);

        private async Task OnGoToStartQuizExecutedAsync(object p)
        {
            var currentUserResultCount = await resultsService.GetResultsCountByStudentIdAsync(AccountStore.CurrentAdminId);

            if (currentUserResultCount > 0)
            {
                // TODO: Error message
                return;
            }

            var quiz = await quizzesService.GetQuizByPasswordAsync<AttemptedQuizViewModel>(QuizPassword);

            if (quiz == null || quiz.EventId == null)
            {
                // TODO: Error message
                return;
            }

            sharedDataStore.QuizToPass = quiz;

            foreach (var question in sharedDataStore.QuizToPass.Questions)
            {
                question.Answers = shuffler.Shuffle(question.Answers);
            }

            sharedDataStore.CurrentResultId = await resultsService.CreateResultAsync(AccountStore.CurrentAdminId, 0, sharedDataStore.QuizToPass.Id);

            startQuizRenavigator.Renavigate();
        }

        #endregion
    }
}
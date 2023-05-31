namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels
{
    using System.Threading.Tasks;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentHomeViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Главная";

        public static IconChar IconChar { get; } = IconChar.Home;

        private readonly IQuizzesService quizzesService;

        private readonly IResultsService resultsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IShuffler shuffler;

        private readonly IRenavigator startQuizRenavigator;

        public StudentHomeViewModel(
            IQuizzesService quizzesService,
            IResultsService resultsService,
            IShuffler shuffler,
            ISharedDataStore sharedDataStore,
            IRenavigator startQuizRenavigator)
        {
            this.quizzesService = quizzesService;
            this.resultsService = resultsService;
            this.shuffler = shuffler;
            this.sharedDataStore = sharedDataStore;
            this.startQuizRenavigator = startQuizRenavigator;

            GoToStartQuizCommandAsync = new ActionCommandAsync(OnGoToStartQuizExecutedAsync, CanGoToStartQuizExecute);
        }

        #region Fields and properties

        private string quizPassword;
        public string QuizPassword
        {
            get => quizPassword;
            set => Set(ref quizPassword, value);
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region GoToStartQuizCommandAsync

        public ICommandAsync GoToStartQuizCommandAsync { get; }

        private bool CanGoToStartQuizExecute(object p) => !string.IsNullOrEmpty(QuizPassword);

        private async Task OnGoToStartQuizExecutedAsync(object p)
        {
            var quizToPass = await quizzesService.GetQuizByPasswordAsync<AttemptedQuizViewModel>(QuizPassword);

            if (quizToPass == null)
            {
                ErrorMessage = "Нет викторины с таким паролем.";
                return;
            }

            if (quizToPass.Event.Id == null)
            {
                ErrorMessage = "Викторина должна быть назначена на событие.";
                return;
            }

            if (quizToPass.Event.Status != Status.Active)
            {
                ErrorMessage = "Событие еще не активно.";
                return;
            }

            var doesParticipantHasResult = await resultsService.DoesParticipantHasResult(sharedDataStore.CurrentUser.Id, quizToPass.Id);

            if (doesParticipantHasResult)
            {
                ErrorMessage = "Вы уже участвовали в викторине.";
                return;
            }

            foreach (var question in quizToPass.Questions)
            {
                question.Answers = shuffler.Shuffle(question.Answers);
            }

            sharedDataStore.QuizToPass = quizToPass;

            startQuizRenavigator.Renavigate();
        }

        #endregion
    }
}
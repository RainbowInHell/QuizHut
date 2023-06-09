namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels
{
    using System.Linq;
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
        public string Title { get; set; } = "Главная";

        public IconChar IconChar { get; set; } = IconChar.Home;

        private readonly IQuizzesService quizzesService;

        private readonly IResultsService resultsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator startQuizRenavigator;

        public StudentHomeViewModel(
            IQuizzesService quizzesService,
            IResultsService resultsService,
            ISharedDataStore sharedDataStore,
            IRenavigator startQuizRenavigator)
        {
            this.quizzesService = quizzesService;
            this.resultsService = resultsService;
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
                ErrorMessage = "Нет викторины с таким паролем";
                return;
            }

            if (!quizToPass.Questions.Any() || quizToPass.Event == null || quizToPass.Event.Status != Status.Active)
            {
                ErrorMessage = "Викторина временно недоступна";
                return;
            }

            var isStudentGroupAssignedToQuiz = await quizzesService.IsQuizAssignedToGroup(sharedDataStore.CurrentUser.Id, quizToPass.Event.Id, quizToPass.Id);

            if (!isStudentGroupAssignedToQuiz)
            {
                ErrorMessage = "Ваша группа не назначена на событие";
                return;
            }

            var doesParticipantHasResult = await resultsService.DoesParticipantHasResult(sharedDataStore.CurrentUser.Id, quizToPass.Id);

            if (doesParticipantHasResult)
            {
                ErrorMessage = "Вы уже участвовали в викторине";
                return;
            }

            sharedDataStore.QuizToPass = quizToPass;

            startQuizRenavigator.Renavigate();
        }

        #endregion
    }
}
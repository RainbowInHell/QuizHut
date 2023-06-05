namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

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

        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator startQuizRenavigator;

        public HomeViewModel(
            IQuizzesService quizzesService,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizRenavigator,
            IRenavigator startQuizRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.quizzesService = quizzesService;
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

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
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
            var quizToPass = await quizzesService.GetQuizByPasswordAsync<AttemptedQuizViewModel>(QuizPassword);

            if (quizToPass == null)
            {
                ErrorMessage = "Нет викторины с таким паролем.";
                return;
            }

            sharedDataStore.QuizToPass = quizToPass;

            startQuizRenavigator.Renavigate();
        }

        #endregion
    }
}
namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Answers;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    class HomeViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Главная";
     
        public static IconChar IconChar { get; } = IconChar.Home;

        private readonly IQuizzesService quizzesService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IShuffler shuffler;

        public HomeViewModel(
            IQuizzesService quizzesService,
            IShuffler shuffler,
            ISharedDataStore sharedDataStore,
            IRenavigator startQuizRenavigator)
        {
            this.quizzesService = quizzesService;
            this.shuffler = shuffler;
            this.sharedDataStore = sharedDataStore;

            NavigatesStartQuizCommand = new RenavigateCommand(startQuizRenavigator);

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

        public ICommand NavigatesStartQuizCommand { get; }

        #endregion

        #region GoToStartQuizCommandAsync

        public ICommandAsync GoToStartQuizCommandAsync { get; }

        private bool CanGoToStartQuizExecute(object p) => !string.IsNullOrEmpty(QuizPassword);

        private async Task OnGoToStartQuizExecutedAsync(object p)
        {
            var quiz = await quizzesService.GetQuizByPasswordAsync<AttemtedQuizViewModel>(QuizPassword);

            if (quiz != null)
            {
                foreach (var question in  quiz.Questions)
                {
                    question.Answers = shuffler.Shuffle<AttemtedQuizAnswerViewModel>(question.Answers);
                }

                sharedDataStore.QuizToPass = quiz;
                NavigatesStartQuizCommand.Execute(p);
            }
        }

        #endregion

    }
}
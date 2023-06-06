namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels.PassingQuizViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class StartQuizViewModel : ViewModel
    {
        private readonly IResultsService resultsService;

        private readonly IShuffler shuffler;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator takingQuizRenavigator;

        private readonly IAccountStore accountStore;

        private readonly IRenavigator homeRenavigator;

        private readonly IRenavigator studentHomeRenavigator;

        public StartQuizViewModel(
            IResultsService resultsService,
            IShuffler shuffler,
            ISharedDataStore sharedDataStore,
            IRenavigator takingQuizRenavigator,
            IAccountStore accountStore,
            IRenavigator homeRenavigator,
            IRenavigator studentHomeRenavigator)
        {
            this.resultsService = resultsService;
            this.shuffler = shuffler;
            this.sharedDataStore = sharedDataStore;
            this.accountStore = accountStore;
            this.takingQuizRenavigator = takingQuizRenavigator;

            this.homeRenavigator = homeRenavigator;
            this.studentHomeRenavigator = studentHomeRenavigator;

            NavigateTakingQuizAsyncCommand = new ActionCommandAsync(OnNavigateTakingQuizAsyncCommandExecute);
            NavigateHomeCommand = new ActionCommand(OnNavigateHomeCommandExecuted);
        }

        #region Fields and properties

        private AttemptedQuizViewModel currentQuiz;
        public AttemptedQuizViewModel CurrentQuiz
        {
            get
            {
                currentQuiz = sharedDataStore.QuizToPass;
                return currentQuiz;
            }
            set => Set(ref currentQuiz, value);
        }

        #endregion

        #region NavigationCommands

        public ICommandAsync NavigateTakingQuizAsyncCommand { get; }

        private async Task OnNavigateTakingQuizAsyncCommandExecute(object p)
        {
            sharedDataStore.QuizToPass.Questions = sharedDataStore.QuizToPass.Questions.OrderBy(q => q.Number).ToList();

            foreach (var question in sharedDataStore.QuizToPass.Questions)
            {
                question.Answers = shuffler.Shuffle(question.Answers);
            }

            sharedDataStore.RemainingTime = TimeSpan.FromMinutes(CurrentQuiz.Timer);
            
            if (sharedDataStore.CurrentUserRole == UserRole.Student)
            {
                sharedDataStore.CurrentResultId = await resultsService.CreateResultAsync(sharedDataStore.CurrentUser.Id, sharedDataStore.QuizToPass.Id);
            }

            takingQuizRenavigator.Renavigate();
        }

        public ICommand NavigateHomeCommand { get; }

        private void OnNavigateHomeCommandExecuted(object p)
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
    }
}
namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels.PassingQuizViewModels
{
    using System;
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

        public StartQuizViewModel(
            IResultsService resultsService,
            IShuffler shuffler,
            ISharedDataStore sharedDataStore,
            IRenavigator takingQuizRenavigator,
            IRenavigator homeRenavigator)
        {
            this.resultsService = resultsService;
            this.shuffler = shuffler;
            this.sharedDataStore = sharedDataStore;
            this.takingQuizRenavigator = takingQuizRenavigator;

            NavigateTakingQuizAsyncCommand = new ActionCommandAsync(OnNavigateTakingQuizAsyncCommandExecute);
            NavigateHomeCommand = new RenavigateCommand(homeRenavigator);
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

        #endregion
    }
}
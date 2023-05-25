namespace QuizHut.ViewModels.MainViewModels.QuizViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class AddEditQuizViewModel : ViewModel
    {
        private readonly IQuizzesService quizzesService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public AddEditQuizViewModel(
            IQuizzesService quizzesService,
            ISharedDataStore sharedDataStore,
            IRenavigator quizRenavigator,
            IRenavigator questionCreateRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.quizzesService = quizzesService;
            this.sharedDataStore = sharedDataStore;
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateQuizCommand = new RenavigateCommand(quizRenavigator);
            NavigateCreateQuestionCommand = new RenavigateCommand(questionCreateRenavigator, ViewDisplayType.Create, viewDisplayTypeService);

            RefreshQuizPasswordCommand = new ActionCommand(OnRefreshQuizPasswordCommandExecutedAsync, CanRefreshQuizPasswordCommandExecute);
            CreateQuizCommandAsync = new ActionCommandAsync(OnCreateQuizCommandExecutedAsync, CanCreateQuizCommandExecute);
            UpdateQuizCommandAsync = new ActionCommandAsync(OnUpdateQuizCommandExecutedAsync, CanUpdateQuizCommandExecute);
        }

        #region Fields and properties

        public ViewDisplayType? CurrentViewDisplayType
        {
            get
            {
                if (viewDisplayTypeService.CurrentViewDisplayType == ViewDisplayType.Edit)
                {
                    QuizNameToCreate = sharedDataStore.SelectedQuiz.Name;
                    QuizPasswordToCreate = sharedDataStore.SelectedQuiz.Password;
                    QuizDescriptionToCreate = sharedDataStore.SelectedQuiz.Description;
                    QuizTimerToCreate = sharedDataStore.SelectedQuiz.Timer;
                }
                else
                {
                    QuizPasswordToCreate = Guid.NewGuid().ToString();
                }

                return viewDisplayTypeService.CurrentViewDisplayType;
            }
        }

        private string quizNameToCreate;
        public string QuizNameToCreate
        {
            get => quizNameToCreate;
            set => Set(ref quizNameToCreate, value);
        }

        private string quizPasswordToCreate;
        public string QuizPasswordToCreate
        {
            get => quizPasswordToCreate;
            set => Set(ref quizPasswordToCreate, value);
        }

        private string quizDescriptionToCreate;
        public string QuizDescriptionToCreate
        {
            get => quizDescriptionToCreate;
            set => Set(ref quizDescriptionToCreate, value);
        }

        private int quizTimerToCreate;
        public int QuizTimerToCreate
        {
            get => quizTimerToCreate;
            set => Set(ref quizTimerToCreate, value);
        }

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizCommand { get; }

        public ICommand NavigateCreateQuestionCommand { get; }

        #endregion

        #region RefreshQuizPasswordCommand

        public ICommand RefreshQuizPasswordCommand { get; }

        private bool CanRefreshQuizPasswordCommandExecute(object p) => true;

        private void OnRefreshQuizPasswordCommandExecutedAsync(object p)
        {
            QuizPasswordToCreate = Guid.NewGuid().ToString();
        }

        #endregion

        #region CreateQuizCommandAsync

        public ICommandAsync CreateQuizCommandAsync { get; }

        private bool CanCreateQuizCommandExecute(object p) => true;

        private async Task OnCreateQuizCommandExecutedAsync(object p)
        {
            var quizWithSamePasswordId = await quizzesService.GetQuizByPasswordAsync<QuizAssignViewModel>(QuizPasswordToCreate);

            if (quizWithSamePasswordId != null)
            {
                // TODO: сообщение об ошибке
                return;
            }

            var quizId = await quizzesService.CreateQuizAsync(
                QuizNameToCreate,
                QuizDescriptionToCreate,
                QuizTimerToCreate, sharedDataStore.CurrentUser.Id,
                QuizPasswordToCreate);

            //
            //sharedDataStore.SelectedQuiz.Id = quizId;

            //NavigateCreateQuestionCommand.Execute(p);
            NavigateQuizCommand.Execute(p);
        }

        #endregion

        #region UpdateQuizCommandAsync

        public ICommandAsync UpdateQuizCommandAsync { get; }

        private bool CanUpdateQuizCommandExecute(object p) => true;

        private async Task OnUpdateQuizCommandExecutedAsync(object p)
        {
            var quizWithSamePassword = await quizzesService.GetQuizByPasswordAsync<QuizAssignViewModel>(QuizPasswordToCreate);

            if (quizWithSamePassword != null && quizWithSamePassword.Id != sharedDataStore.SelectedQuiz.Id)
            {
                // TODO: сообщение об ошибке
                return;
            }

            await quizzesService.UpdateQuizAsync(
                sharedDataStore.SelectedQuiz.Id,
                QuizNameToCreate,
                QuizDescriptionToCreate,
                QuizTimerToCreate,
                QuizPasswordToCreate);

            NavigateQuizCommand.Execute(p);
        }

        #endregion
    }
}
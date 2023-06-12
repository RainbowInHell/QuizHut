namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.QuizViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

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

            RefreshQuizPasswordCommand = new ActionCommand(OnRefreshQuizPasswordCommandExecutedAsync);
            CreateQuizCommandAsync = new ActionCommandAsync(OnCreateQuizCommandExecutedAsync, CanCreateUpdateQuizCommandExecute);
            UpdateQuizCommandAsync = new ActionCommandAsync(OnUpdateQuizCommandExecutedAsync, CanCreateUpdateQuizCommandExecute);
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

        private string? createUpdateErrorMessage;
        public string? CreateUpdateErrorMessage
        {
            get => createUpdateErrorMessage;
            set => Set(ref createUpdateErrorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateQuizCommand { get; }

        public ICommand NavigateCreateQuestionCommand { get; }

        #endregion

        #region RefreshQuizPasswordCommand

        public ICommand RefreshQuizPasswordCommand { get; }

        private void OnRefreshQuizPasswordCommandExecutedAsync(object p)
        {
            QuizPasswordToCreate = Guid.NewGuid().ToString();
        }

        #endregion

        #region CreateQuizCommandAsync

        public ICommandAsync CreateQuizCommandAsync { get; }

        private bool CanCreateUpdateQuizCommandExecute(object p)
        {
            if (string.IsNullOrEmpty(QuizNameToCreate) || 
                string.IsNullOrEmpty(QuizPasswordToCreate) || 
                string.IsNullOrEmpty(QuizDescriptionToCreate))
            {
                CreateUpdateErrorMessage = "Все поля должны быть заполнены";
                return false;
            }

            CreateUpdateErrorMessage = null;
            return true;
        }

        private async Task OnCreateQuizCommandExecutedAsync(object p)
        {
            var quizWithSamePasswordId = await quizzesService.GetQuizByPasswordAsync<QuizAssignViewModel>(QuizPasswordToCreate);

            if (quizWithSamePasswordId != null)
            {
                CreateUpdateErrorMessage = "Викторина с таким паролем уже существует";
                return;
            }

            await quizzesService.CreateQuizAsync(
                QuizNameToCreate,
                QuizDescriptionToCreate,
                QuizTimerToCreate, sharedDataStore.CurrentUser.Id,
                QuizPasswordToCreate);

            NavigateQuizCommand.Execute(p);
        }

        #endregion

        #region UpdateQuizCommandAsync

        public ICommandAsync UpdateQuizCommandAsync { get; }

        private async Task OnUpdateQuizCommandExecutedAsync(object p)
        {
            var quizWithSamePassword = await quizzesService.GetQuizByPasswordAsync<QuizAssignViewModel>(QuizPasswordToCreate);

            if (quizWithSamePassword != null && quizWithSamePassword.Id != sharedDataStore.SelectedQuiz.Id)
            {
                CreateUpdateErrorMessage = "Викторина с таким паролем уже существует";
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
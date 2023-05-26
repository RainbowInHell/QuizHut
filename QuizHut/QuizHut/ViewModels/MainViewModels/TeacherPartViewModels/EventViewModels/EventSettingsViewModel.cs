namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.EventViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

    class EventSettingsViewModel : ViewModel
    {
        private readonly IEventsService eventsService;

        private readonly IQuizzesService quizzesService;

        private readonly IGroupsService groupsService;

        private readonly ISharedDataStore sharedDataStore;

        public EventSettingsViewModel(
            IEventsService eventsService,
            IQuizzesService quizzesService,
            IGroupsService groupsService,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizzesRenavigator,
            IRenavigator addGroupsRenavigator,
            IRenavigator quizSettingRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            this.eventsService = eventsService;
            this.quizzesService = quizzesService;
            this.groupsService = groupsService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuizzesCommand = new RenavigateCommand(addQuizzesRenavigator, ViewDisplayType.AddQuizzes, groupSettingsTypeService);
            NavigateAddGroupsCommand = new RenavigateCommand(addGroupsRenavigator, ViewDisplayType.AddGroups, groupSettingsTypeService);
            NavigateQuizSettingsCommand = new RenavigateCommand(quizSettingRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SendEmailWithQuizPasswordCommandAsync = new ActionCommandAsync(OnSendEmailWithQuizPasswordCommandExecuteAsync, CanSendEmailWithQuizPasswordCommandExecute);
            DeleteEventFromGroupCommandAsync = new ActionCommandAsync(OnDeleteEventFromGroupCommandExecutedAsync, CanDeleteEventFromGroupCommandExecute);
            DeleteQuizFromEventCommandAsync = new ActionCommandAsync(OnDeleteQuizFromEventCommandExecutedAsync, CanDeleteQuizFromEventCommandExecute);
        }

        #region NavigationCommands

        public ICommand NavigateAddQuizzesCommand { get; }

        public ICommand NavigateAddGroupsCommand { get; }

        public ICommand NavigateQuizSettingsCommand { get; }

        #endregion

        #region Fields and properties

        public ObservableCollection<QuizAssignViewModel> quizzes;
        public ObservableCollection<QuizAssignViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        public ObservableCollection<GroupAssignViewModel> groups;
        public ObservableCollection<GroupAssignViewModel> Groups
        {
            get => groups;
            set => Set(ref groups, value);
        }

        private QuizAssignViewModel selectedQuiz;
        public QuizAssignViewModel SelectedQuiz
        {
            get
            {
                sharedDataStore.SelectedQuiz = new() { Id = selectedQuiz?.Id };
                return selectedQuiz;
            }
            set => Set(ref selectedQuiz, value);
        }

        private GroupAssignViewModel selectedGroup;
        public GroupAssignViewModel SelectedGroup
        {
            get => selectedGroup;
            set => Set(ref selectedGroup, value);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();

            await LoadGroupsData();
        }

        #endregion

        #region SendEmailWithQuizPasswordCommandAsync

        public ICommandAsync SendEmailWithQuizPasswordCommandAsync { get; }

        private bool CanSendEmailWithQuizPasswordCommandExecute(object p) => true;

        private async Task OnSendEmailWithQuizPasswordCommandExecuteAsync(object p)
        {
            await eventsService.SendEmailsToEventGroups(sharedDataStore.SelectedEvent.Id, SelectedQuiz.Id);
        }

        #endregion

        #region DeleteEventFromGroupCommandAsync

        public ICommandAsync DeleteEventFromGroupCommandAsync { get; }

        private bool CanDeleteEventFromGroupCommandExecute(object p) => true;

        private async Task OnDeleteEventFromGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteEventFromGroupAsync(SelectedGroup.Id, sharedDataStore.SelectedEvent.Id);

            await LoadGroupsData();
        }

        #endregion

        #region DeleteQuizFromEventCommandAsync

        public ICommandAsync DeleteQuizFromEventCommandAsync { get; }

        private bool CanDeleteQuizFromEventCommandExecute(object p) => true;

        private async Task OnDeleteQuizFromEventCommandExecutedAsync(object p)
        {
            await eventsService.DeleteQuizFromEventAsync(sharedDataStore.SelectedEvent.Id, SelectedQuiz.Id);

            await LoadQuizzesData();
        }

        #endregion

        private async Task LoadQuizzesData()
        {
            var quizzes = await quizzesService.GetQuizzesByEventId<QuizAssignViewModel>(sharedDataStore.SelectedEvent.Id);

            Quizzes = new(quizzes);
        }

        private async Task LoadGroupsData()
        {
            var groups = await groupsService.GetAllGroupsByEventIdAsync<GroupAssignViewModel>(sharedDataStore.SelectedEvent.Id);

            Groups = new(groups);
        }
    }
}
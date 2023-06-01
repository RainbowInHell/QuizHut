namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.EventViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class EventActionsViewModel : ViewModel
    {
        private readonly IEventsService eventsService;

        private readonly IQuizzesService quizzesService;

        private readonly IGroupsService groupsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public EventActionsViewModel(
            IEventsService eventsService,
            IQuizzesService quizzesService,
            IGroupsService groupsService,
            ISharedDataStore sharedDataStore,
            IRenavigator eventRenavigator,
            IRenavigator eventSettingRenavigator,
            IRenavigator addQuizRenavigator,
            IRenavigator addGroupRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.eventsService = eventsService;
            this.quizzesService = quizzesService;
            this.groupsService = groupsService;
            this.sharedDataStore = sharedDataStore;
            this.viewDisplayTypeService = viewDisplayTypeService;

            viewDisplayTypeService.StateChanged += ViewDisplayTypeService_StateChanged; ;

            NavigateEventCommand = new RenavigateCommand(eventRenavigator);
            NavigateEventSettingsCommand = new RenavigateCommand(eventSettingRenavigator);
            NavigateAddQuizCommand = new RenavigateCommand(addQuizRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateAddGroupCommand = new RenavigateCommand(addGroupRenavigator, ViewDisplayType.Create, viewDisplayTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            CreateEventCommandAsync = new ActionCommandAsync(OnCreateEventCommandExecutedAsync, CanCreateEventCommandExecute);
            UpdateEventCommandAsync = new ActionCommandAsync(OnUpdateEventCommandExecutedAsync, CanUpdateEventCommandExecute);
            AssignQuizToEventCommandAsync = new ActionCommandAsync(OnAssignQuizToEventCommandExecute, CanAssignQuizToEventCommandExecute);
            AssignGroupsToEventCommandAsync = new ActionCommandAsync(OnAssignGroupsToEventCommandExecute, CanAssignGroupsToEventCommandExecute);
        }

        #region Fields and properties

        private bool isQuizzesEmpty;
        public bool IsQuizzesEmpty
        {
            get => isQuizzesEmpty;
            set => Set(ref isQuizzesEmpty, value);
        }

        private bool isGroupsEmpty;
        public bool IsGroupsEmpty
        {
            get => isGroupsEmpty;
            set => Set(ref isGroupsEmpty, value);
        }

        public ViewDisplayType? CurrentViewDisplayType
        {
            get
            {
                if (viewDisplayTypeService.CurrentViewDisplayType == ViewDisplayType.Edit)
                {
                    var timeParts = sharedDataStore.SelectedEvent.Duration.Split('-');

                    EventNameToCreate = sharedDataStore.SelectedEvent.Name;
                    EventActivationDate = sharedDataStore.SelectedEvent.StartDate;
                    EventAvaliableFrom = timeParts[0].Trim();
                    EventAvaliableTo = timeParts[1].Trim();
                }

                return viewDisplayTypeService.CurrentViewDisplayType;
            }
        }

        private string eventNameToCreate;
        public string EventNameToCreate
        {
            get => eventNameToCreate;
            set => Set(ref eventNameToCreate, value);
        }

        private string eventActivationDate;
        public string EventActivationDate
        {
            get => eventActivationDate;
            set => Set(ref eventActivationDate, value);
        }

        private string eventAvaliableFrom;
        public string EventAvaliableFrom
        {
            get => eventAvaliableFrom;
            set => Set(ref eventAvaliableFrom, value);
        }

        private string eventAvaliableTo;
        public string EventAvaliableTo
        {
            get => eventAvaliableTo;
            set => Set(ref eventAvaliableTo, value);
        }

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

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateEventCommand { get; }

        public ICommand NavigateEventSettingsCommand { get; }

        public ICommand NavigateAddQuizCommand { get; }

        public ICommand NavigateAddGroupCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p)
        {
            if (CurrentViewDisplayType != ViewDisplayType.Create
                &&
                CurrentViewDisplayType != ViewDisplayType.Edit)
            {
                return true;
            }

            return false;
        }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            if (CurrentViewDisplayType == ViewDisplayType.AddQuizzes)
            {
                await LoadQuizzesData();
            }

            if (CurrentViewDisplayType == ViewDisplayType.AddGroups)
            {
                await LoadGroupsData();
            }
        }

        #endregion

        #region CreateEventCommandAsync

        public ICommandAsync CreateEventCommandAsync { get; }

        private bool CanCreateEventCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(EventNameToCreate) ||
                !string.IsNullOrEmpty(EventActivationDate) ||
                !string.IsNullOrEmpty(EventAvaliableFrom) ||
                !string.IsNullOrEmpty(EventAvaliableTo))
            {
                return true;
            }

            return false;
        }

        private async Task OnCreateEventCommandExecutedAsync(object p)
        {
            var timeErrorMessage = eventsService.GetTimeErrorMessage(EventAvaliableFrom, eventAvaliableTo, EventActivationDate);

            if (timeErrorMessage != null)
            {
                // TODO: Error message for user
                return;
            }

            await eventsService.CreateEventAsync(EventNameToCreate, EventActivationDate, EventAvaliableFrom, EventAvaliableTo, sharedDataStore.CurrentUser.Id);

            NavigateEventCommand.Execute(p);
        }

        #endregion

        #region UpdateEventCommandAsync

        public ICommandAsync UpdateEventCommandAsync { get; }

        private bool CanUpdateEventCommandExecute(object p)
        {
            if (!string.IsNullOrEmpty(EventNameToCreate) ||
                !string.IsNullOrEmpty(EventActivationDate) ||
                !string.IsNullOrEmpty(EventAvaliableFrom) ||
                !string.IsNullOrEmpty(EventAvaliableTo))
            {
                return true;
            }

            return false;
        }

        private async Task OnUpdateEventCommandExecutedAsync(object p)
        {
            var timeErrorMessage = eventsService.GetTimeErrorMessage(EventAvaliableFrom, eventAvaliableTo, EventActivationDate);

            if (timeErrorMessage != null)
            {
                // TODO: Error message for user
                return;
            }

            await eventsService.UpdateEventAsync(sharedDataStore.SelectedEvent.Id, EventNameToCreate, EventActivationDate, EventAvaliableFrom, EventAvaliableTo);

            NavigateEventCommand.Execute(p);
        }

        #endregion

        #region AssignQuizToEventCommandAsync

        public ICommandAsync AssignQuizToEventCommandAsync { get; }

        private bool CanAssignQuizToEventCommandExecute(object p) => true;

        private async Task OnAssignQuizToEventCommandExecute(object p)
        {
            var selectedQuizes = Quizzes.Where(s => s.IsAssigned).Select(x => x.Id).ToList();

            if (selectedQuizes.Count() == 0)
            {
                // TODO: Error message for user
                return;
            }

            await eventsService.AssignQuizzesToEventAsync(selectedQuizes, sharedDataStore.SelectedEvent.Id);

            NavigateEventSettingsCommand.Execute(p);
        }

        #endregion

        #region AssignGroupsToEventCommandAsync

        public ICommandAsync AssignGroupsToEventCommandAsync { get; }

        private bool CanAssignGroupsToEventCommandExecute(object p) => true;

        private async Task OnAssignGroupsToEventCommandExecute(object p)
        {
            var selectedGroupIds = Groups.Where(s => s.IsAssigned).Select(x => x.Id).ToList();

            if (selectedGroupIds.Count() == 0)
            {
                // TODO: Error message for user
                return;
            }

            await eventsService.AssignGroupsToEventAsync(selectedGroupIds, sharedDataStore.SelectedEvent.Id);

            NavigateEventSettingsCommand.Execute(p);
        }

        #endregion

        private async Task LoadQuizzesData()
        {
            var quizzes = await quizzesService.GetUnAssignedQuizzesToEventAsync<QuizAssignViewModel>(sharedDataStore.CurrentUser.Id);

            Quizzes = new(quizzes);
            IsQuizzesEmpty = quizzes.Count == 0;
        }

        private async Task LoadGroupsData()
        {
            var groups = await groupsService.GetAllGroupsAsync<GroupAssignViewModel>(sharedDataStore.CurrentUser.Id, sharedDataStore.SelectedEvent.Id);

            Groups = new(groups);
            IsGroupsEmpty = groups.Count == 0;
        }

        private void ViewDisplayTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewDisplayType));
        }

        public override void Dispose()
        {
            viewDisplayTypeService.StateChanged -= ViewDisplayTypeService_StateChanged;

            base.Dispose();
        }
    }
}
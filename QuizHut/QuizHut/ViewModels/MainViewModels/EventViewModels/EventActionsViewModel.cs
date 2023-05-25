namespace QuizHut.ViewModels.MainViewModels.EventViewModels
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

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            CreateEventCommandAsync = new ActionCommandAsync(OnCreateEventCommandExecutedAsync, CanCreateEventCommandExecute);
            UpdateEventCommandAsync = new ActionCommandAsync(OnUpdateEventCommandExecutedAsync, CanUpdateEventCommandExecute);
            AssignQuizToEventCommandAsync = new ActionCommandAsync(OnAssignQuizToEventCommandExecute, CanAssignQuizToEventCommandExecute);
            AssignGroupsToEventCommandAsync = new ActionCommandAsync(OnAssignGroupsToEventCommandExecute, CanAssignGroupsToEventCommandExecute);
        }

        #region Fields and properties

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

        #endregion

        #region NavigationCommands

        public ICommand NavigateEventCommand { get; }

        public ICommand NavigateEventSettingsCommand { get; }

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

            await eventsService.CreateEventAsync(EventNameToCreate, EventActivationDate, EventAvaliableFrom, EventAvaliableTo, AccountStore.CurrentAdminId);

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
            var quizzes = await quizzesService.GetUnAssignedQuizzesToEventAsync<QuizAssignViewModel>(AccountStore.CurrentAdminId);

            Quizzes = new(quizzes);
        }

        private async Task LoadGroupsData()
        {
            var groups = await groupsService.GetAllGroupsAsync<GroupAssignViewModel>(AccountStore.CurrentAdminId, sharedDataStore.SelectedEvent.Id);

            Groups = new(groups);
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
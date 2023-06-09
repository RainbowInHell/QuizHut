namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.EventViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
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

        private const string datePattern = @"^(0[1-9]|1\d|2\d|3[01])/(0[1-9]|1[0-2])/\d{4}$";

        private const string timePattern = @"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$";

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
            CreateEventCommandAsync = new ActionCommandAsync(OnCreateEventCommandExecutedAsync, CanCreateUpdateEventCommandExecute);
            UpdateEventCommandAsync = new ActionCommandAsync(OnUpdateEventCommandExecutedAsync, CanCreateUpdateEventCommandExecute);
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

        public ObservableCollection<QuizAssignViewModel> quizzes = new ();
        public ObservableCollection<QuizAssignViewModel> Quizzes
        {
            get => quizzes;
            set => Set(ref quizzes, value);
        }

        public ObservableCollection<GroupAssignViewModel> groups = new();
        public ObservableCollection<GroupAssignViewModel> Groups
        {
            get => groups;
            set => Set(ref groups, value);
        }

        private string? createUpdateErrorMessage;
        public string? CreateUpdateErrorMessage
        {
            get => createUpdateErrorMessage;
            set => Set(ref createUpdateErrorMessage, value);
        }

        private string? errorMessageQuizzes;
        public string? ErrorMessageQuizzes
        {
            get => errorMessageQuizzes;
            set => Set(ref errorMessageQuizzes, value);
        }

        private string? errorMessageGroups;
        public string? ErrorMessageGroups
        {
            get => errorMessageGroups;
            set => Set(ref errorMessageGroups, value);
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
            if (CurrentViewDisplayType == ViewDisplayType.AddQuizzes
                ||
                CurrentViewDisplayType == ViewDisplayType.AddGroups)
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

        private bool CanCreateUpdateEventCommandExecute(object p)
        {
            if (string.IsNullOrEmpty(EventNameToCreate) ||
                string.IsNullOrEmpty(EventActivationDate) ||
                string.IsNullOrEmpty(EventAvaliableFrom) ||
                string.IsNullOrEmpty(EventAvaliableTo))
            {
                CreateUpdateErrorMessage = "Все поля должны быть заполнены";
                return false;
            }

            if (!Regex.IsMatch(EventActivationDate, datePattern))
            {
                CreateUpdateErrorMessage = "Неверный формат даты";
                return false;
            }

            if (!Regex.IsMatch(EventAvaliableFrom, timePattern) || !Regex.IsMatch(EventAvaliableTo, timePattern))
            {
                CreateUpdateErrorMessage = "Неверный формат времени";
                return false;
            }

            var timeErrorMessage = eventsService.GetTimeErrorMessage(EventAvaliableFrom, EventAvaliableTo, EventActivationDate);

            if (timeErrorMessage != null)
            {
                CreateUpdateErrorMessage = timeErrorMessage;
                return false;
            }

            CreateUpdateErrorMessage = null;
            return true;
        }

        private async Task OnCreateEventCommandExecutedAsync(object p)
        {
            await eventsService.CreateEventAsync(EventNameToCreate, EventActivationDate, EventAvaliableFrom, EventAvaliableTo, sharedDataStore.CurrentUser.Id);

            NavigateEventCommand.Execute(p);
        }

        #endregion

        #region UpdateEventCommandAsync

        public ICommandAsync UpdateEventCommandAsync { get; }

        private async Task OnUpdateEventCommandExecutedAsync(object p)
        {
            var timeParts = sharedDataStore.SelectedEvent.Duration.Split('-');
            var oldEventAvaliableFrom = timeParts[0].Trim();

            var timeErrorMessage = eventsService.GetTimeErrorMessage(EventAvaliableFrom, EventAvaliableTo, EventActivationDate, oldEventAvaliableFrom);

            if (timeErrorMessage != null)
            {
                CreateUpdateErrorMessage = timeErrorMessage;
                return;
            }

            await eventsService.UpdateEventAsync(sharedDataStore.SelectedEvent.Id, EventNameToCreate, EventActivationDate, EventAvaliableFrom, EventAvaliableTo);

            NavigateEventCommand.Execute(p);
        }

        #endregion

        #region AssignQuizToEventCommandAsync

        public ICommandAsync AssignQuizToEventCommandAsync { get; }

        private bool CanAssignQuizToEventCommandExecute(object p)
        {
            if (sharedDataStore.SelectedEvent != null)
            {
                if (sharedDataStore.SelectedEvent.Status == Status.Ended)
                {
                    ErrorMessageQuizzes = "Нельзя назначить викторину завершенному событию";
                    return false;
                }
            }

            if (!Quizzes.Where(s => s.IsAssigned).Any())
            {
                ErrorMessageQuizzes = "Выберите хотя бы одну викторину";
                return false;
            }

            ErrorMessageQuizzes = null;
            return true;
        }

        private async Task OnAssignQuizToEventCommandExecute(object p)
        {
            var selectedQuizes = Quizzes.Where(s => s.IsAssigned).Select(x => x.Id).ToList();

            await eventsService.AssignQuizzesToEventAsync(selectedQuizes, sharedDataStore.SelectedEvent.Id);

            NavigateEventSettingsCommand.Execute(p);
        }

        #endregion

        #region AssignGroupsToEventCommandAsync

        public ICommandAsync AssignGroupsToEventCommandAsync { get; }

        private bool CanAssignGroupsToEventCommandExecute(object p)
        {
            if (sharedDataStore.SelectedEvent != null)
            {
                if (sharedDataStore.SelectedEvent.Status == Status.Ended)
                {
                    ErrorMessageGroups = "Нельзя назначить группу завершенному событию";
                    return false;
                }
            }

            if (!Groups.Where(s => s.IsAssigned).Any())
            {
                ErrorMessageGroups = "Выберите хотя бы одну группу";
                return false;
            }

            ErrorMessageGroups = null;
            return true;
        }

        private async Task OnAssignGroupsToEventCommandExecute(object p)
        {
            var selectedGroupIds = Groups.Where(s => s.IsAssigned).Select(x => x.Id).ToList();

            await eventsService.AssignGroupsToEventAsync(selectedGroupIds, sharedDataStore.SelectedEvent.Id);

            NavigateEventSettingsCommand.Execute(p);
        }

        #endregion

        private async Task LoadQuizzesData()
        {
            var quizzes = await quizzesService.GetUnAssignedQuizzesToEventAsync<QuizAssignViewModel>(sharedDataStore.CurrentUser.Id);

            IsQuizzesEmpty = !quizzes.Any();

            if (!IsQuizzesEmpty)
            {
                Quizzes = new(quizzes);
            }
        }

        private async Task LoadGroupsData()
        {
            var groups = await groupsService.GetAllGroupsAsync<GroupAssignViewModel>(sharedDataStore.CurrentUser.Id, sharedDataStore.SelectedEvent.Id);

            IsGroupsEmpty = !groups.Any();

            if (!IsGroupsEmpty)
            {
                Groups = new(groups);
            }
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
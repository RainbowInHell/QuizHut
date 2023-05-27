namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.ResultViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class ActiveEndedEventsViewModel : ViewModel
    {
        private readonly IEventsService eventsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public ActiveEndedEventsViewModel(
            IEventsService eventsService, 
            ISharedDataStore sharedDataStore,
            IRenavigator resultsForEventRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.eventsService = eventsService;
            this.sharedDataStore = sharedDataStore;
            this.viewDisplayTypeService = viewDisplayTypeService;

            NavigateResultsForEventCommand = new RenavigateCommand(resultsForEventRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
        }

        #region FieldsAndProperties

        public ViewDisplayType? CurrentViewDisplayType => viewDisplayTypeService.CurrentViewDisplayType;

        public ObservableCollection<EventSimpleViewModel> events;
        public ObservableCollection<EventSimpleViewModel> Events
        {
            get => events;
            set => Set(ref events, value);
        }

        private EventSimpleViewModel eventToView;
        public EventSimpleViewModel EventToView
        {
            get
            {
                sharedDataStore.EventToView = eventToView;
                return eventToView;
            }
            set => Set(ref eventToView, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region NavigationCommands

        public ICommand NavigateResultsForEventCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            if (CurrentViewDisplayType == ViewDisplayType.ActiveEvents)
            {
                await LoadActiveEventsDataAsync(Status.Active);
            }
            else
            {
                await LoadActiveEventsDataAsync(Status.Ended);
            }
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => !string.IsNullOrEmpty(SearchText);

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            if (CurrentViewDisplayType == ViewDisplayType.ActiveEvents)
            {
                await LoadActiveEventsDataAsync(Status.Active,SearchText);
            }
            else
            {
                await LoadActiveEventsDataAsync(Status.Ended, SearchText);
            }
        }

        #endregion

        private async Task LoadActiveEventsDataAsync(Status status, string searchText = null)
        {
            var events = await eventsService.GetAllEventsByCreatorIdAndStatusAsync<EventSimpleViewModel>(status, sharedDataStore.CurrentUser.Id, searchText);

            if (!events.Any())
            {
                ErrorMessage = "Нет событий.";
            }

            Events = new(events);
        }
    }
}
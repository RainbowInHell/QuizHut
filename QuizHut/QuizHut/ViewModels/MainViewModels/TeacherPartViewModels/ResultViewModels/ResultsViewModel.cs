namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.ResultViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResultsViewModel : ViewModel, IMenuView
    {
        public string Title { get; set; } = "Результаты"; 
        
        public IconChar IconChar { get; set; } = IconChar.Award;

        private readonly IEventsService eventsService;

        private readonly ISharedDataStore sharedDataStore;

        public ResultsViewModel(
            IEventsService eventsService,
            ISharedDataStore sharedDataStore,
            IRenavigator activeEndedEventsRenavigator,
            IRenavigator resultsForEventRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.eventsService = eventsService;
            this.sharedDataStore = sharedDataStore;

            NavigateActiveEventsCommand = new RenavigateCommand(activeEndedEventsRenavigator, ViewDisplayType.ActiveEvents, viewDisplayTypeService);
            NavigateEndedEventsCommand = new RenavigateCommand(activeEndedEventsRenavigator, ViewDisplayType.EndedEvents, viewDisplayTypeService);
            NavigateResultsForEventCommand = new RenavigateCommand(resultsForEventRenavigator);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
        }

        #region FieldsAndProperties

        private ObservableCollection<EventSimpleViewModel> activeEvents;
        public ObservableCollection<EventSimpleViewModel> ActiveEvents
        {
            get => activeEvents;
            set => Set(ref activeEvents, value);
        }

        private ObservableCollection<EventSimpleViewModel> endedEvents;
        public ObservableCollection<EventSimpleViewModel> EndedEvents
        {
            get => endedEvents;
            set => Set(ref endedEvents, value);
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

        #endregion

        #region NavigationCommands

        public ICommand NavigateActiveEventsCommand { get; }

        public ICommand NavigateEndedEventsCommand { get; }

        public ICommand NavigateResultsForEventCommand { get; }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadActiveEventsDataAsync();

            await LoadEndedEventsDataAsync();
        }

        #endregion

        private async Task LoadActiveEventsDataAsync()
        {
            var activeEvents = await eventsService.GetAllEventsByCreatorIdAndStatusAsync<EventSimpleViewModel>(Status.Active, sharedDataStore.CurrentUser.Id);

            ActiveEvents = new(activeEvents);
        }

        private async Task LoadEndedEventsDataAsync()
        {
            var endedEvents = await eventsService.GetAllEventsByCreatorIdAndStatusAsync<EventSimpleViewModel>(Status.Ended, sharedDataStore.CurrentUser.Id);

            EndedEvents = new(endedEvents);
        }
    }
}
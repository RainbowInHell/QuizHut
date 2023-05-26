namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.ResultViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class ActiveEventsViewModel : ViewModel
    {
        private readonly IEventsService eventsService;
        
        private readonly ISharedDataStore sharedDataStore;

        public ActiveEventsViewModel(IEventsService eventsService, ISharedDataStore sharedDataStore)
        {
            this.eventsService = eventsService;
            this.sharedDataStore = sharedDataStore;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
        }

        #region FieldsAndProperties

        public ObservableCollection<EventSimpleViewModel> activeEvents;
        public ObservableCollection<EventSimpleViewModel> ActiveEvents
        {
            get => activeEvents;
            set => Set(ref activeEvents, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadActiveEventsDataAsync();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => !string.IsNullOrEmpty(SearchText);

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadActiveEventsDataAsync(SearchText);
        }

        #endregion

        private async Task LoadActiveEventsDataAsync(string searchText = null)
        {
            var activeEvents = await eventsService.GetAllEventsByCreatorIdAndStatusAsync<EventSimpleViewModel>(Status.Active, sharedDataStore.CurrentUser.Id, searchText);

            ActiveEvents = new(activeEvents);
        }
        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }
    }
}
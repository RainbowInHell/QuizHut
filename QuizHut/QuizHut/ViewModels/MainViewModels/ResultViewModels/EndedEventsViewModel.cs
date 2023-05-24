namespace QuizHut.ViewModels.MainViewModels.ResultViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.ViewModels.Base;

    class EndedEventsViewModel : ViewModel
    {
        private readonly IEventsService eventsService;

        public EndedEventsViewModel(IEventsService eventsService)
        {
            this.eventsService = eventsService;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
        }

        #region FieldsAndProperties

        public ObservableCollection<EventSimpleViewModel> endedEvents;
        public ObservableCollection<EventSimpleViewModel> EndedEvents
        {
            get => endedEvents;
            set => Set(ref endedEvents, value);
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

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadEndedEventsDataAsync();
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => !string.IsNullOrEmpty(SearchText);

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadEndedEventsDataAsync(SearchText);
        }

        #endregion

        private async Task LoadEndedEventsDataAsync(string searchText = null)
        {
            var endedEvents = await eventsService.GetAllEventsByCreatorIdAndStatusAsync<EventSimpleViewModel>(Status.Ended, AccountStore.CurrentAdminId, searchText);

            EndedEvents = new(endedEvents);
        }
    }
}
namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentPendingEventsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "В ожидании";
        public static IconChar IconChar { get; } = IconChar.HourglassStart;

        private readonly IEventsService eventsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IDateTimeConverter dateTimeConverter;

        public StudentPendingEventsViewModel(IEventsService eventsService, ISharedDataStore sharedDataStore, IDateTimeConverter dateTimeConverter)
        {
            this.eventsService = eventsService;
            this.sharedDataStore = sharedDataStore;
            this.dateTimeConverter = dateTimeConverter;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute, CanSearchCommandAsyncExecute);
        }

        #region Fields and properties

        public ObservableCollection<StudentPendingEventViewModel> studentPendingEvents;
        public ObservableCollection<StudentPendingEventViewModel> StudentPendingEvents
        {
            get => studentPendingEvents;
            set => Set(ref studentPendingEvents, value);
        }

        private string searchCriteria;
        public string SearchCriteria
        {
            get => searchCriteria;
            set => Set(ref searchCriteria, value);
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        #endregion

        #region SearchCommandAsync

        public ICommandAsync SearchCommandAsync { get; }

        private bool CanSearchCommandAsyncExecute(object p) => SearchCriteria != null && SearchText != null;

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            //await LoadStudentResultsAsync(SearchCriteriasInEnglish[SearchCriteria], SearchText);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadStudentPendingEventsAsync();
        }

        #endregion

        private async Task LoadStudentPendingEventsAsync(string searchCriteria = null, string searchText = null)
        {
            var studentPendingEvents = await eventsService.GetAllEventsByStatusAndStudentIdAsync<StudentPendingEventViewModel>(Status.Pending, sharedDataStore.CurrentUser.Id, searchCriteria, searchText);

            foreach (var studentActiveEvent in studentPendingEvents)
            {
                studentActiveEvent.Duration = dateTimeConverter.GetDurationString(studentActiveEvent.ActivationDateAndTime, studentActiveEvent.DurationOfActivity);
                studentActiveEvent.Date = dateTimeConverter.GetDate(studentActiveEvent.ActivationDateAndTime);
            }

            StudentPendingEvents = new(studentPendingEvents);
        }
    }

}
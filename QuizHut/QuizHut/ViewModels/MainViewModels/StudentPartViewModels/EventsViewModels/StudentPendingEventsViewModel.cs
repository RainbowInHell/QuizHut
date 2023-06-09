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
        public string Title { get; set; } = "В ожидании";

        public IconChar IconChar { get; set; } = IconChar.HourglassStart;

        private readonly IEventsService eventsService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IDateTimeConverter dateTimeConverter;

        private readonly IExporter exporter;

        public StudentPendingEventsViewModel(
            IEventsService eventsService, 
            ISharedDataStore sharedDataStore, 
            IDateTimeConverter dateTimeConverter,
            IExporter exporter)
        {
            this.eventsService = eventsService;
            this.sharedDataStore = sharedDataStore;
            this.dateTimeConverter = dateTimeConverter;
            this.exporter = exporter;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            ExportDataAsyncCommand = new ActionCommandAsync(OnExportDataAsyncCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute);
        }

        #region Fields and properties

        public ObservableCollection<StudentPendingEventViewModel> studentPendingEvents;
        public ObservableCollection<StudentPendingEventViewModel> StudentPendingEvents
        {
            get => studentPendingEvents;
            set => Set(ref studentPendingEvents, value);
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

        private async Task OnSearchCommandAsyncExecute(object p)
        {
            await LoadStudentPendingEventsAsync(SearchText);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadStudentPendingEventsAsync();
        }

        #endregion

        #region ExportDataCommand

        public ICommandAsync ExportDataAsyncCommand { get; }

        private async Task OnExportDataAsyncCommandExecute(object p)
        {
            await exporter.GenerateExcelReportAsync(studentPendingEvents);
        }

        #endregion

        private async Task LoadStudentPendingEventsAsync(string searchText = null)
        {
            var studentPendingEvents = await eventsService.GetAllEventsByStatusAndStudentIdAsync<StudentPendingEventViewModel>(Status.Pending, sharedDataStore.CurrentUser.Id, searchText);

            foreach (var studentActiveEvent in studentPendingEvents)
            {
                studentActiveEvent.Duration = dateTimeConverter.GetDurationString(studentActiveEvent.ActivationDateAndTime, studentActiveEvent.DurationOfActivity);
                studentActiveEvent.Date = dateTimeConverter.GetDate(studentActiveEvent.ActivationDateAndTime);
            }

            StudentPendingEvents = new(studentPendingEvents);
        }
    }

}
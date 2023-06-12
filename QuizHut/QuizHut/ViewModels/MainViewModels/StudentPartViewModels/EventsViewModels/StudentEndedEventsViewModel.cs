namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.EntityViewModels.Results;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentEndedEventsViewModel : ViewModel, IMenuView
    {
        public string Title { get; set; } = "Завершённые";

        public IconChar IconChar { get; set; } = IconChar.HourglassEnd;

        private readonly IEventsService eventsService;

        private readonly IResultsService resultsService;

        private readonly IDateTimeConverter dateTimeConverter;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IExporter exporter;

        public StudentEndedEventsViewModel(
            IEventsService eventsService,
            IResultsService resultsService,
            ISharedDataStore sharedDataStore, 
            IDateTimeConverter dateTimeConverter,
            IExporter exporter)
        {
            this.eventsService = eventsService;
            this.resultsService = resultsService;
            this.sharedDataStore = sharedDataStore;
            this.dateTimeConverter = dateTimeConverter;
            this.sharedDataStore = sharedDataStore;
            this.exporter = exporter;

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            ExportDataAsyncCommand = new ActionCommandAsync(OnExportDataAsyncCommandExecute);
            SearchCommandAsync = new ActionCommandAsync(OnSearchCommandAsyncExecute);
        }

        #region Fields and properties

        public ObservableCollection<StudentEndedEventViewModel> studentEndedEvents;
        public ObservableCollection<StudentEndedEventViewModel> StudentEndedEvents
        {
            get => studentEndedEvents;
            set => Set(ref studentEndedEvents, value);
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
            await LoadStudentEndedEventsAsync(SearchText);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadStudentEndedEventsAsync();
        }

        #endregion

        #region ExportDataCommand

        public ICommandAsync ExportDataAsyncCommand { get; }

        private async Task OnExportDataAsyncCommandExecute(object p)
        {
            await exporter.GenerateExcelReportAsync(studentEndedEvents);
        }

        #endregion

        private async Task LoadStudentEndedEventsAsync(string searchText = null)
        {
            var studentEndedEvents = await eventsService.GetAllEventsByStatusAndStudentIdAsync<StudentEndedEventViewModel>(Status.Ended, sharedDataStore.CurrentUser.Id, searchText);

            var studentResults = await resultsService.GetAllResultsByStudentIdAsync<ScoreViewModel>(sharedDataStore.CurrentUser.Id);

            foreach (var studentEndedEvent in studentEndedEvents)
            {
                studentEndedEvent.Date = dateTimeConverter.GetDate(studentEndedEvent.ActivationDateAndTime);

                foreach (var quiz in studentEndedEvent.Quizzes)
                {
                    quiz.Score = studentResults.FirstOrDefault(x => x.QuizId == quiz.Id);
                }
            }

            StudentEndedEvents = new(studentEndedEvents);
        }
    }
}
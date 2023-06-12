namespace QuizHut.ViewModels.MainViewModels.TeacherPartViewModels.ResultViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Results;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResultsForEventViewModel : ViewModel, IView
    {
        public string Title { get; set; }

        private readonly IResultsService resultsService;

        private readonly IGroupsService groupsService;

        private readonly ISharedDataStore sharedDataStore;

        public ResultsForEventViewModel(
            IResultsService resultsService,
            IGroupsService groupsService, 
            ISharedDataStore sharedDataStore)
        {
            this.resultsService = resultsService;
            this.groupsService = groupsService;
            this.sharedDataStore = sharedDataStore;

            LoadEventGroupsDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync);
            LoadEventResultsForGroupCommandAsync = new ActionCommandAsync(OnLoadEventResultsForGroupCommandAsync, CanLoadEventResultsForGroupCommandExecute);
            DeleteResultCommandAsync = new ActionCommandAsync(OnDeleteResultCommandExecuteAsync);

            Title = $"Результаты события '{sharedDataStore.EventToView.Name}'";
        }

        #region FieldsAndProperties

        private ObservableCollection<GroupSimpleViewModel> eventGroups;
        public ObservableCollection<GroupSimpleViewModel> EventGroups
        {
            get => eventGroups;
            set => Set(ref eventGroups, value);
        }

        private ObservableCollection<ResultViewModel> eventResults;
        public ObservableCollection<ResultViewModel> EventResults
        {
            get => eventResults;
            set => Set(ref eventResults, value);
        }

        private GroupSimpleViewModel selectedGroup;
        public GroupSimpleViewModel SelectedGroup
        {
            get => selectedGroup;
            set => Set(ref selectedGroup, value);
        }       
        
        private ResultViewModel selectedResult;
        public ResultViewModel SelectedResult
        {
            get => selectedResult;
            set => Set(ref selectedResult, value);
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => Set(ref errorMessage, value);
        }

        #endregion

        #region LoadEventGroupsDataCommandAsync

        public ICommandAsync LoadEventGroupsDataCommandAsync { get; }

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadEventGroupsDataAsync();
        }

        #endregion

        #region LoadEventResultsForGroupCommandAsync

        public ICommandAsync LoadEventResultsForGroupCommandAsync { get; }

        private bool CanLoadEventResultsForGroupCommandExecute(object p) => SelectedGroup != null;

        private async Task OnLoadEventResultsForGroupCommandAsync(object p)
        {
            await LoadEventResultsForGroupAsync();
        }

        #endregion

        #region DeleteResultCommandAsync

        public ICommandAsync DeleteResultCommandAsync { get; }

        private async Task OnDeleteResultCommandExecuteAsync(object p)
        {
            await resultsService.DeleteResultAsync(SelectedResult.Id);

            await LoadEventResultsForGroupAsync();
        }

        #endregion

        private async Task LoadEventGroupsDataAsync()
        {
            var eventGroups = await groupsService.GetAllGroupsByEventIdAsync<GroupSimpleViewModel>(sharedDataStore.EventToView.Id);

            EventGroups = new(eventGroups);
        }
        
        private async Task LoadEventResultsForGroupAsync()
        {
            var eventResults = await resultsService.GetAllResultsByEventAndGroupAsync<ResultViewModel>(sharedDataStore.EventToView.Id, SelectedGroup.Id);

            EventResults = new(eventResults);
        }
    }
}
﻿namespace QuizHut.ViewModels.MainViewModels.EventViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using System.Linq;

    class EventSettingsViewModel : ViewModel
    {
        private readonly IEventsService eventsService;

        private readonly IQuizzesService quizzesService;

        private readonly IGroupsService groupsService;

        private readonly ISharedDataStore sharedDataStore;

        public EventSettingsViewModel(
            IEventsService eventsService,
            IQuizzesService quizzesService,
            IGroupsService groupsService,
            ISharedDataStore sharedDataStore,
            IRenavigator addQuizzesRenavigator,
            IRenavigator addGroupsRenavigator,
            IViewDisplayTypeService groupSettingsTypeService)
        {
            this.eventsService = eventsService;
            this.quizzesService = quizzesService;
            this.groupsService = groupsService;
            this.sharedDataStore = sharedDataStore;

            NavigateAddQuizzesCommand = new RenavigateCommand(addQuizzesRenavigator, ViewDisplayType.AddQuizzes, groupSettingsTypeService);
            NavigateAddGroupsCommand = new RenavigateCommand(addGroupsRenavigator, ViewDisplayType.AddGroups, groupSettingsTypeService);

            LoadDataCommandAsync = new ActionCommandAsync(OnLoadDataCommandExecutedAsync, CanLoadDataCommandExecute);
            DeleteEventFromGroupCommandAsync = new ActionCommandAsync(OnDeleteEventFromGroupCommandExecutedAsync, CanDeleteEventFromGroupCommandExecute);
            DeleteQuizFromEventCommandAsync = new ActionCommandAsync(OnDeleteQuizFromEventCommandExecutedAsync, CanDeleteQuizFromEventCommandExecute);
        }

        #region NavigationCommands

        public ICommand NavigateAddQuizzesCommand { get; }

        public ICommand NavigateAddGroupsCommand { get; }

        #endregion

        #region Fields and properties

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

        private QuizAssignViewModel selectedQuiz;
        public QuizAssignViewModel SelectedQuiz
        {
            get => selectedQuiz;
            set => Set(ref selectedQuiz, value);
        }

        private GroupAssignViewModel selectedGroup;
        public GroupAssignViewModel SelectedGroup
        {
            get => selectedGroup;
            set => Set(ref selectedGroup, value);
        }

        #endregion

        #region LoadDataCommandAsync

        public ICommandAsync LoadDataCommandAsync { get; }

        private bool CanLoadDataCommandExecute(object p) => true;

        private async Task OnLoadDataCommandExecutedAsync(object p)
        {
            await LoadQuizzesData();

            await LoadGroupsData();
        }

        #endregion

        #region DeleteEventFromGroupCommandAsync

        public ICommandAsync DeleteEventFromGroupCommandAsync { get; }

        private bool CanDeleteEventFromGroupCommandExecute(object p) => true;

        private async Task OnDeleteEventFromGroupCommandExecutedAsync(object p)
        {
            await groupsService.DeleteEventFromGroupAsync(SelectedGroup.Id, sharedDataStore.SelectedEventId);

            await LoadGroupsData();
        }

        #endregion

        #region DeleteQuizFromEventCommandAsync

        public ICommandAsync DeleteQuizFromEventCommandAsync { get; }

        private bool CanDeleteQuizFromEventCommandExecute(object p) => true;

        private async Task OnDeleteQuizFromEventCommandExecutedAsync(object p)
        {
            await eventsService.DeleteQuizFromEventAsync(sharedDataStore.SelectedEventId, SelectedQuiz.Id);

            await LoadQuizzesData();
        }

        #endregion

        private async Task LoadQuizzesData()
        {
            var quizz = await quizzesService.GetQuizByEventId<QuizAssignViewModel>(sharedDataStore.SelectedEventId);
            
            Quizzes = new();

            if (quizz != null)
            {
                Quizzes.Add(quizz);
            }
            else
            {
                Quizzes.DefaultIfEmpty();
            }
        }

        private async Task LoadGroupsData()
        {
            var groups = await groupsService.GetAllByEventIdAsync<GroupAssignViewModel>(sharedDataStore.SelectedEventId);

            Groups = new(groups);
        }
    }
}
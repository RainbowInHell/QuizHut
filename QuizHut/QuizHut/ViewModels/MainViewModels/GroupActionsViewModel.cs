namespace QuizHut.ViewModels.MainViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using QuizHut.BLL.Helpers;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class GroupActionsViewModel : ViewModel
    {
        private readonly IGroupsService groupsService;

        private readonly IGroupSettingsTypeService groupSettingsTypeService;

        private readonly ISharedDataStore sharedDataStore;

        public GroupActionsViewModel(
            IGroupsService groupsService,
            IRenavigator groupRenavigator,
            IGroupSettingsTypeService groupSettingsTypeService,
            ISharedDataStore sharedDataStore)
        {
            this.groupsService = groupsService;
            this.groupSettingsTypeService = groupSettingsTypeService;
            this.sharedDataStore = sharedDataStore;

            groupSettingsTypeService.StateChanged += GroupSettingsTypeService_StateChanged;

            NavigateGroupCommand = new RenavigateCommand(groupRenavigator);

            CreateGroupCommandAsync = new ActionCommandAsync(OnCreateGroupCommandExecutedAsync, CanCreateGroupCommandExecute);
            UpdateGroupNameCommandAsync = new ActionCommandAsync(OnUpdateGroupNameCommandExecutedAsync, CanUpdateGroupNameCommandExecute);
        }

        #region Fields and properties

        public GroupViewType? GroupViewType => groupSettingsTypeService.GroupViewType;

        private string groupNameToCreate;
        public string GroupNameToCreate
        {
            get => groupNameToCreate;
            set => Set(ref groupNameToCreate, value);
        }

        #endregion

        #region NavigateGroupCommand

        public ICommand NavigateGroupCommand { get; }

        #endregion

        #region CreateGroupCommand

        public ICommandAsync CreateGroupCommandAsync { get; }

        private bool CanCreateGroupCommandExecute(object p) => !string.IsNullOrEmpty(GroupNameToCreate);

        private async Task OnCreateGroupCommandExecutedAsync(object p)
        {
            await groupsService.CreateGroupAsync(GroupNameToCreate, AccountStore.CurrentAdminId);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        #region UpdateGroupNameCommand

        public ICommandAsync UpdateGroupNameCommandAsync { get; }

        private bool CanUpdateGroupNameCommandExecute(object p) => !string.IsNullOrEmpty(GroupNameToCreate);

        private async Task OnUpdateGroupNameCommandExecutedAsync(object p)
        {
            await groupsService.UpdateNameAsync(sharedDataStore.SelectedGroupId, GroupNameToCreate);

            NavigateGroupCommand.Execute(p);
        }

        #endregion

        private void GroupSettingsTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(GroupViewType));
        }

        public override void Dispose()
        {
            groupSettingsTypeService.StateChanged -= GroupSettingsTypeService_StateChanged;

            base.Dispose();
        }
    }
}
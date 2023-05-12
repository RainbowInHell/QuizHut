namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class CreateGroupViewModel : ViewModel
    {
        private readonly IGroupSettingsTypeService groupSettingsTypeService;

        public CreateGroupViewModel(IRenavigator groupRenavigator, IGroupSettingsTypeService groupSettingsTypeService) 
        { 
            this.groupSettingsTypeService = groupSettingsTypeService;

            groupSettingsTypeService.StateChanged += GroupSettingsTypeService_StateChanged;

            NavigateGroupCommand = new RenavigateCommand(groupRenavigator);
        }

        #region Fields and properties

        public GroupViewType? GroupViewType => groupSettingsTypeService.GroupViewType;

        #endregion

        #region Commands

        public ICommand NavigateGroupCommand { get; }

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
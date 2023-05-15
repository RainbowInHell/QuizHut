namespace QuizHut.ViewModels.MainViewModels.EventViewModels
{
    using System.Windows.Input;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    class EventActionsViewModel : ViewModel
    {
        private readonly IViewDisplayTypeService viewDisplayTypeService;

        public EventActionsViewModel(
            IRenavigator eventRenavigator,
            IRenavigator eventSettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            this.viewDisplayTypeService = viewDisplayTypeService;

            viewDisplayTypeService.StateChanged += ViewDisplayTypeService_StateChanged; ;

            NavigateEventCommand = new RenavigateCommand(eventRenavigator);
            NavigateEventSettingsCommand = new RenavigateCommand(eventSettingRenavigator);
        }

        #region Fields and properties

        public ViewDisplayType? ViewDisplayType => viewDisplayTypeService.ViewDisplayType;

        #endregion

        #region NavigationCommands

        public ICommand NavigateEventCommand { get; }

        public ICommand NavigateEventSettingsCommand { get; }

        #endregion

        private void ViewDisplayTypeService_StateChanged()
        {
            OnPropertyChanged(nameof(ViewDisplayType));
        }

        public override void Dispose()
        {
            viewDisplayTypeService.StateChanged -= ViewDisplayTypeService_StateChanged;

            base.Dispose();
        }
    }
}
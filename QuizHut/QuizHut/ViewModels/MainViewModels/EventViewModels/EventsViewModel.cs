namespace QuizHut.ViewModels.MainViewModels.EventViewModels
{
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class EventsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "События";
        public static IconChar IconChar { get; } = IconChar.CalendarDays;

        public EventsViewModel(
            IRenavigator eventActionsRenavigator,
            IRenavigator eventSettingRenavigator,
            IViewDisplayTypeService viewDisplayTypeService)
        {
            NavigateCreateEventCommand = new RenavigateCommand(eventActionsRenavigator, ViewDisplayType.Create, viewDisplayTypeService);
            NavigateEditEventCommand = new RenavigateCommand(eventActionsRenavigator, ViewDisplayType.Edit, viewDisplayTypeService);
            NavigateEventSettingsCommand = new RenavigateCommand(eventSettingRenavigator);
        }

        #region NavigationCommands

        public ICommand NavigateCreateEventCommand { get; }

        public ICommand NavigateEditEventCommand { get; }

        public ICommand NavigateEventSettingsCommand { get; }

        #endregion
    }
}
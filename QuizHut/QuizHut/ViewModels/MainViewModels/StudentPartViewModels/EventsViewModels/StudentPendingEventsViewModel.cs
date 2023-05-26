namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentPendingEventsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "В ожидании";

        public static IconChar IconChar { get; } = IconChar.HourglassStart;
    }
}
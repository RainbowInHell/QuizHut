namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentActiveEventsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Активные";

        public static IconChar IconChar { get; } = IconChar.HourglassHalf;
    }
}
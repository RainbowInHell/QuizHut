namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels.EventsViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentEndedEventsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Оконченные";

        public static IconChar IconChar { get; } = IconChar.HourglassEnd;
    }
}
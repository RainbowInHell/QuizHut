namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class EventsViewModel : ViewModel
    {
        public static string Title { get; } = "События";
        public static IconChar IconChar { get; } = IconChar.CalendarDays;
    }
}
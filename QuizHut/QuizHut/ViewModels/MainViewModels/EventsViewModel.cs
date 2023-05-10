namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class EventsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "События";
        public static IconChar IconChar { get; } = IconChar.CalendarDays;
    }
}
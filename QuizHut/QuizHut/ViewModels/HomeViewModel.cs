namespace QuizHut.ViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class HomeViewModel : ViewModel
    {
        public static string Title { get; } = "Главная";
        public static IconChar IconChar { get; } = IconChar.Home;
    }
}
namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    class ResultsViewModel : ViewModel
    {
        public static string Title { get; } = "Результаты";
        public static IconChar IconChar { get; } = IconChar.Award;
    }
}
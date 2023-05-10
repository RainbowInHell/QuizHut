namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class ResultsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Результаты"; 
        public static IconChar IconChar { get; } = IconChar.Award;
    }
}
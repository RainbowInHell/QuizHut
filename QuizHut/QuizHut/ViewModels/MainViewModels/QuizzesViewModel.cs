namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class QuizzesViewModel : ViewModel
    {
        public static string Title { get; } = "Викторины";
        public static IconChar IconChar { get; } = IconChar.FolderOpen;
    }
}
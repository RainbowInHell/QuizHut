namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class QuizzesViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Викторины";
        public static IconChar IconChar { get; } = IconChar.FolderOpen;
    }
}
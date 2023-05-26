namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class StudentHomeViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Главная";

        public static IconChar IconChar { get; } = IconChar.Home;
    }
}

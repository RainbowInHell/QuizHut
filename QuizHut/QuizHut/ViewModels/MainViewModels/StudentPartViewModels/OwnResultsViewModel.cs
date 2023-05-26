namespace QuizHut.ViewModels.MainViewModels.StudentPartViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class OwnResultsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Мои результаты";

        public static IconChar IconChar { get; } = IconChar.Trophy;
    }
}

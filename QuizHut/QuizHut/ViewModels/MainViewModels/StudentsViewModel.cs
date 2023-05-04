namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class StudentsViewModel : ViewModel
    {
        public static string Title { get; } = "Учащиеся";
        public static IconChar IconChar { get; } = IconChar.UserGroup;
    }
}
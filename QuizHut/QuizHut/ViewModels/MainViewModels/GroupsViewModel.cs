namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class GroupsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Группы";
        public static IconChar IconChar { get; } = IconChar.PeopleGroup;
    }
}
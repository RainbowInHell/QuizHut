namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class GroupsViewModel : ViewModel
    {
        public static string Title { get; } = "Группы";
        public static IconChar IconChar { get; } = IconChar.PeopleGroup;
    }
}
namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class CategoriesViewModel : ViewModel
    {
        public static string Title { get; } = "Категории";
        public static IconChar IconChar { get; } = IconChar.LayerGroup;
    }
}
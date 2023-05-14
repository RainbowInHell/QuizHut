namespace QuizHut.ViewModels.MainViewModels.CategoryViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class CategoriesViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Категории";
        public static IconChar IconChar { get; } = IconChar.LayerGroup;
    }
}
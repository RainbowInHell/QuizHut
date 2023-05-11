namespace QuizHut.ViewModels.Contracts
{
    using FontAwesome.Sharp;

    interface IMenuView
    {
        static string Title { get; }

        static IconChar IconChar { get; }
    }
}
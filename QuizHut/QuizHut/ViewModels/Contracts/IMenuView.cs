namespace QuizHut.ViewModels.Contracts
{
    using FontAwesome.Sharp;

    interface IMenuView : IView
    {
        static IconChar IconChar { get; }
    }
}
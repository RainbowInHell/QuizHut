namespace QuizHut.ViewModels.MainViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class UserProfileViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Профиль"; 
        public static IconChar IconChar { get; } = IconChar.AddressCard;
    }
}
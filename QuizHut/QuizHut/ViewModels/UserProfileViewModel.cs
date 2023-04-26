namespace QuizHut.ViewModels
{
    using FontAwesome.Sharp;

    using QuizHut.ViewModels.Base;

    class UserProfileViewModel : ViewModel
    {
        public static string Title { get; } = "Профиль";
        public static IconChar IconChar { get; } = IconChar.AddressCard;
    }
}
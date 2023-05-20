namespace QuizHut.BLL.Helpers
{
    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.DLL.Entities;

    public class AccountStore : IAccountStore
    {
        public static string CurrentAdminId;

        private ApplicationUser currentUser;
        public ApplicationUser CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                StateChanged?.Invoke();
            }
        }

        private bool isLoggedIn;
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                isLoggedIn = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
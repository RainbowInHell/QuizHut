namespace QuizHut.BLL.Helpers
{
    using QuizHut.BLL.Helpers.Contracts;

    public class AccountStore : IAccountStore
    {
        public static string CurrentAdminId;

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
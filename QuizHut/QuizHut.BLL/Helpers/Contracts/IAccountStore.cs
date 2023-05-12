namespace QuizHut.BLL.Helpers.Contracts
{
    public interface IAccountStore
    {
        public static string CurrentAdminId;

        bool IsLoggedIn { get; set; }

        event Action StateChanged;
    }
}
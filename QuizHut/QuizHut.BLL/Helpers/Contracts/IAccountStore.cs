using QuizHut.DLL.Entities;

namespace QuizHut.BLL.Helpers.Contracts
{
    public interface IAccountStore
    {
        public static string CurrentAdminId;

        ApplicationUser CurrentUser { get; set; }

        bool IsLoggedIn { get; set; }

        event Action StateChanged;
    }
}
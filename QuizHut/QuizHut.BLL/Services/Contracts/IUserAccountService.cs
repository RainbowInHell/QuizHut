namespace QuizHut.BLL.Services.Contracts
{
    using System.Threading.Tasks;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.DLL.Entities;

    public interface IUserAccountService
    {
        IAccountStore CurrentUser { get; set; }

        Task<bool> RegisterAsync(ApplicationUser newUser, string password);

        Task<bool> LoginAsync(string email, string password);

        Task<string> SendPasswordResetEmail(string email);

        Task<bool> ResetUserPassword(string email, string resetToken, string newPassword);

        void Logout();
    }
}
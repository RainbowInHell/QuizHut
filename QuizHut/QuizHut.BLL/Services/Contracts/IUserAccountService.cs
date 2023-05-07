namespace QuizHut.BLL.Services.Contracts
{
    using System.Security.Principal;
    using System.Threading.Tasks;

    using QuizHut.DAL.Entities;

    public interface IUserAccountService
    {
        bool IsLoggedIn { get; }
        Task<bool> RegisterAsync(ApplicationUser newUser, string password);

        Task<bool> LoginAsync(string email, string password);

        Task<string> SendPasswordResetEmail(string email);

        Task<bool> ResetUserPassword(string email, string resetToken, string newPassword);
    }
}
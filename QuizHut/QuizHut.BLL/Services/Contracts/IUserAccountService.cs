namespace QuizHut.BLL.Services.Contracts
{
    using System.Threading.Tasks;

    using QuizHut.DAL.Entities;

    public interface IUserAccountService
    {
        Task<bool> RegisterAsync(ApplicationUser newUser, string password);

        Task<bool> LoginAsync(string email, string password);

        Task<bool> SendPasswordResetEmail(string email);

        Task<bool> ResetUserPassword(string email, string resetToken, string newPassword);
    }
}
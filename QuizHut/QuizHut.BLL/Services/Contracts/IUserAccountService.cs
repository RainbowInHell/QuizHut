namespace QuizHut.BLL.Services.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.DLL.Entities;

    public interface IUserAccountService
    {
        Task<bool> RegisterAsync(ApplicationUser newUser, string password, UserRole userRole);

        Task<bool> LoginAsync(string email, string password);

        Task<string> SendPasswordResetEmail(string email);

        Task<bool> ResetUserPassword(string email, string resetToken, string newPassword);

        void Logout();

        Task<ApplicationUser> UpdateUserAsync(ApplicationUser existingUser);

        Task<IdentityResult> DeleteUserAsync(string id);
    }
}
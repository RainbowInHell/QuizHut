namespace QuizHut.Services.Contracts
{
    using System.Threading.Tasks;

    using QuizHut.DAL.Entities;

    public interface IAuthService
    {
        Task<bool> RegisterAsync(ApplicationUser newUser, string password);

        Task<bool> LoginAsync(string email, string password);

        Task<bool> ResetPasswordAsync(string email);
    }
}
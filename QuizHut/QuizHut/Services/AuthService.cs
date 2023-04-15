namespace QuizHut.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    using QuizHut.DAL.Entities;
    using QuizHut.Services.Contracts;

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> RegisterAsync(ApplicationUser newUser, string password)
        {
            var appUser = new ApplicationUser
            {
                UserName = "Sparf",
                Email = "Sparf",
                FirstName = "Sparf",
                LastName = "Sparf"
            };

            var result = await userManager.CreateAsync(appUser, "!A#1dfg");

            return result.Succeeded;
        }

        //Sparf, "!A#1dfggg"
        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var isTokenValid = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, resetToken);

            var result = await userManager.ResetPasswordAsync(user, resetToken, "!A#1dfggg");

            return true;
        }
    }
}
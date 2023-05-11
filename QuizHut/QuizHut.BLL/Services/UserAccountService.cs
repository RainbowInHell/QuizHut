namespace QuizHut.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DAL.Entities;

    public class UserAccountService : IUserAccountService
    {
        private IAccountStore accountStore;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IEmailSenderService emailSender;

        public UserAccountService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender, IAccountStore accountStore)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.accountStore = accountStore;
        }

        public IAccountStore CurrentUser 
        {
            get => accountStore;
            set => accountStore = value;
        }

        public async Task<bool> RegisterAsync(ApplicationUser newUser, string password)
        {
            newUser.UserName = newUser.Email;

            var result = await userManager.CreateAsync(newUser, password);

            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var isRightPassword = await userManager.CheckPasswordAsync(user, password);

            CurrentUser.IsLoggedIn = isRightPassword ? true : false;

            return isRightPassword;
        }

        public async Task<string> SendPasswordResetEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var sendEmailResponse = await emailSender.SendEmailAsync(email, "Password reset token", resetToken);

            if (!sendEmailResponse.IsSuccessStatusCode)
            {
                return null;
            }

            return resetToken;
        }

        public async Task<bool> ResetUserPassword(string email, string resetToken, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var isTokenValid = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, resetToken);

            if (!isTokenValid)
            {
                return false;
            }

            var result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);

            return result.Succeeded;
        }

        public void Logout()
        {
            CurrentUser.IsLoggedIn = false;
        }
    }
}
namespace QuizHut.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;

    public class UserAccountService : IUserAccountService
    {        
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IEmailSenderService emailSender;
        
        private readonly IAccountStore accountStore;

        public UserAccountService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender, IAccountStore accountStore)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.accountStore = accountStore;
        }

        public async Task<bool> RegisterAsync(ApplicationUser newUser, string password, UserRole userRole)
        {
            newUser.UserName = newUser.Email;

            var result = await userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                var roleAssignResult = new IdentityResult();

                if (userRole == UserRole.Teacher)
                {
                    roleAssignResult = await userManager.AddToRoleAsync(newUser, "Organizer");
                }
                else
                {
                    roleAssignResult = await userManager.AddToRoleAsync(newUser, "Student");
                }

                return roleAssignResult.Succeeded;
            }

            return false;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var isRightPassword = await userManager.CheckPasswordAsync(user, password);

            if (isRightPassword)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("Organizer"))
                {
                    accountStore.CurrentUserRole = UserRole.Teacher;
                }
                else
                {
                    accountStore.CurrentUserRole = UserRole.Student;
                }

                accountStore.CurrentUser = user;

                return true;
            }

            return false;
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
            accountStore.CurrentUserRole = UserRole.Unauthorised;
            accountStore.CurrentUser = null;
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser updatedUser)
        {
            var user = await userManager.FindByIdAsync(updatedUser.Id);

            if (user == null)
            {
                return null;
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                accountStore.CurrentUser = updatedUser;
                return user;
            }

            return null;
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            return await userManager.DeleteAsync(user);
        }
    }
}
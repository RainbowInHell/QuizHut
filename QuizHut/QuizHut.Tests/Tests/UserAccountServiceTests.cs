namespace QuizHut.Tests.Tests
{
    using System.Net;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using QuizHut.BLL.Helpers.Contracts;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Services;

    using SendGrid;
    
    using Xunit;

    public class UserAccountServiceTests
    {
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IEmailSenderService> mockEmailSender;
        private Mock<IAccountStore> mockAccountStore;
        private UserAccountService userAccountService;

        public UserAccountServiceTests()
        {
            mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            mockEmailSender = new Mock<IEmailSenderService>();
            mockAccountStore = new Mock<IAccountStore>();

            userAccountService = new UserAccountService(
                mockUserManager.Object,
                mockEmailSender.Object,
                mockAccountStore.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_ValidUserAndPassword_ReturnsTrue()
        {
            // Arrange 
            var newUser = new ApplicationUser();
            var password = "password";

            mockUserManager.Setup(x => x.CreateAsync(newUser, password))
                .ReturnsAsync(IdentityResult.Success);

            // Act 
            var result = await userAccountService.RegisterAsync(newUser, password);

            // Assert 
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterAsync_InvalidUserAndPassword_ReturnsFalse()
        {
            // Arrange 
            var newUser = new ApplicationUser();
            var password = "password";

            mockUserManager.Setup(x => x.CreateAsync(newUser, password))
                .ReturnsAsync(IdentityResult.Failed());

            // Act 
            var result = await userAccountService.RegisterAsync(newUser, password);

            // Assert 
            Assert.False(result);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTrue()
        {
            // Arrange 
            var email = "test@example.com";
            var password = "password";
            var user = new ApplicationUser();

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);

            // Act 
            var result = await userAccountService.LoginAsync(email, password);

            // Assert 
            Assert.True(result);
            //Assert.Equal(user.Id, AccountStore.CurrentAdminId);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsFalse()
        {
            // Arrange 
            var email = "test@example.com";
            var password = "password";

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser)null);

            // Act 
            var result = await userAccountService.LoginAsync(email, password);

            // Assert 
            Assert.False(result);
            //Assert.False(userAccountService.CurrentUser.IsLoggedIn);
            //Assert.Null(userAccountService.CurrentUser.CurrentUser);
            //Assert.Null(AccountStore.CurrentAdminId);
        }

        [Fact]
        public async Task SendPasswordResetEmail_ValidEmail_ReturnsResetToken()
        {
            // Arrange 
            var email = "test@example.com";
            var user = new ApplicationUser();

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("resetToken");
            mockEmailSender.Setup(x => x.SendEmailAsync(email, "Password reset token", "resetToken"))
                .ReturnsAsync(new Response(HttpStatusCode.OK, null, null));

            // Act 
            var result = await userAccountService.SendPasswordResetEmail(email);

            // Assert 
            Assert.Equal("resetToken", result);
        }

        [Fact]
        public async Task SendPasswordResetEmail_InvalidEmail_ReturnsNull()
        {
            // Arrange 
            var email = "test@example.com";

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser)null);

            // Act 
            var result = await userAccountService.SendPasswordResetEmail(email);

            // Assert 
            Assert.Null(result);
        }

        [Fact]
        public async Task ResetUserPassword_ValidData_ReturnsTrue()
        {
            // Arrange 
            var email = "test@example.com";
            var resetToken = "resetToken";
            var newPassword = "newPassword";
            var user = new ApplicationUser();

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);
            mockUserManager.Setup(x => x.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, resetToken))
                .ReturnsAsync(true);
            mockUserManager.Setup(x => x.ResetPasswordAsync(user, resetToken, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act 
            var result = await userAccountService.ResetUserPassword(email, resetToken, newPassword);

            // Assert 
            Assert.True(result);
        }

        [Fact]
        public async Task ResetUserPassword_InvalidData_ReturnsFalse()
        {
            // Arrange 
            var email = "test@example.com";
            var resetToken = "resetToken";
            var newPassword = "newPassword";

            mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser)null);
            mockUserManager.Setup(x => x.VerifyUserTokenAsync(null, TokenOptions.DefaultEmailProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, resetToken))
                .ReturnsAsync(false);

            // Act 
            var result = await userAccountService.ResetUserPassword(email, resetToken, newPassword);

            // Assert 
            Assert.False(result);
        }

        //[Fact]
        //public void Logout_UpdatesCurrentUserIsLoggedInToFalse()
        //{
        //    // Arrange 
        //    userAccountService.AccountStore.CurrentUser = new ApplicationUser();

        //    // Act 
        //    userAccountService.Logout();

        //    // Assert 
        //    Assert.Null(userAccountService.AccountStore.CurrentUser);
        //}
    }
}

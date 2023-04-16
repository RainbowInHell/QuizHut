namespace QuizHut.BLL.Services.Contracts
{
    using System.Threading.Tasks;

    using SendGrid;

    public interface IEmailSenderService
    {
        Task<Response> SendEmailAsync(string toEmail, string subject, string body);
    }
}
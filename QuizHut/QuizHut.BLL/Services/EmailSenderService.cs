namespace QuizHut.BLL.Services
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using QuizHut.BLL.Services.Contracts;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSenderService : IEmailSenderService
    {
        private readonly ISendGridClient sendGridClient;

        private readonly IConfiguration configuration;

        public EmailSenderService(ISendGridClient sendGridClient, IConfiguration configuration)
        {
            this.sendGridClient = sendGridClient;
            this.configuration = configuration;
        }

        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new SendGridMessage()
            {
                From = new EmailAddress(configuration.GetValue<string>("EmailSender:FromEmail")),
                Subject = subject,
                PlainTextContent = body
            };

            message.AddTo(new EmailAddress(toEmail));

            return await sendGridClient.SendEmailAsync(message);
        }
    }
}
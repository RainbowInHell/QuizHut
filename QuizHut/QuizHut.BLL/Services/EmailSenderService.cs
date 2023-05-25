namespace QuizHut.BLL.Services
{
    using System.Threading.Tasks;

    using QuizHut.BLL.Services.Contracts;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSenderService : IEmailSenderService
    {
        private readonly ISendGridClient sendGridClient;

        public EmailSenderService(ISendGridClient sendGridClient)
        {
            this.sendGridClient = sendGridClient;
        }

        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new SendGridMessage()
            {
                From = new EmailAddress(Environment.GetEnvironmentVariable("QH_SENDGRID_FROM_EMAIL", EnvironmentVariableTarget.User)),
                Subject = subject,
                PlainTextContent = body
            };

            message.AddTo(new EmailAddress(toEmail));

            return await sendGridClient.SendEmailAsync(message);
        }
    }
}
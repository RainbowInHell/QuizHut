namespace QuizHut.BLL.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using QuizHut.BLL.Services.Contracts;

    using SendGrid;
    using SendGrid.Helpers.Mail;


    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration configuration;

        private readonly ISendGridClient client;

        private readonly IStringEncoderDecoder stringEncoderDecoder;

        private const string EmailSenderSectionName = "EmailSender:";

        public const string ApiKey = $"{EmailSenderSectionName}ApiKey";

        public const string FromEmail = $"{EmailSenderSectionName}FromEmail";

        public const string FromName = $"{EmailSenderSectionName}FromName";

        public EmailSenderService(IConfiguration configuration, ISendGridClient client, IStringEncoderDecoder stringEncoderDecoder)
        {
            this.configuration = configuration;
            this.client = client;
            this.stringEncoderDecoder = stringEncoderDecoder;
        }

        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body)
        {
            //var apiKey = stringEncoderDecoder.Decode(configuration.GetValue<string>(ApiKey));
            var apiKey = "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";

            var fromEmail = configuration.GetValue<string>(FromEmail);

            var fromName = configuration.GetValue<string>(FromName);

            if (!IsEmailSettingsValid(apiKey, fromEmail, fromName))
            {
                throw new Exception("Email can't be sent. Invalid settings.");
            }

            var message = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = body
            };

            message.AddTo(new EmailAddress(toEmail));

            return await client.SendEmailAsync(message);
        }

        private bool IsEmailSettingsValid(params string[] settings)
        {
            return settings.All(x => !string.IsNullOrEmpty(x));
        }
    }
}
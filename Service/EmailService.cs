using BackendLaboratory.Data.Mailing;
using BackendLaboratory.Service.IService;
using MimeKit;
using MailKit.Net.Smtp;

namespace BackendLaboratory.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(AppConstants.EmailFrom, _emailConfig.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder 
            { 
                HtmlBody = string.Format(AppConstants.MailBody, message.CommunityName, 
                message.ContentTitle, message.ContentDescription)

            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    await client.SendAsync(mailMessage);

                    Console.WriteLine($"Email отправлен на адрес: {mailMessage.To}");
                }

                catch (Exception ex)
                {
                    throw new Exception("Не удалось отправить сообщение:", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}

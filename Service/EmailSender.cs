using BackendLaboratory.Data.Mailing;
using BackendLaboratory.Service.IService;
using MimeKit;
using MailKit.Net.Smtp;

namespace BackendLaboratory.Service
{
    public class EmailSender : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            Console.WriteLine("SendEmailAsync вызван");
            Console.WriteLine(message.To);
            Console.WriteLine(message.Content);
            Console.WriteLine(message.Content);
            

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder 
            { 
                HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content)
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false);

                    await client.SendAsync(mailMessage);

                    Console.WriteLine("Ну, должно было отправиться");
            }
        }
    }
}

using MimeKit;
using System.Xml.Linq;

namespace BackendLaboratory.Data.Mailing
{
    public class Message
    {
        public MailboxAddress To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(string to, string subject, string content)
        {
            To = new MailboxAddress("email", to);
            Subject = subject;
            Content = content;
        }
    }
}

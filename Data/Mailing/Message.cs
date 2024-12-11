using MimeKit;

namespace BackendLaboratory.Data.Mailing
{
    public class Message
    {
        public MailboxAddress To { get; set; }
        public string Subject { get; set; }

        public string CommunityName { get; set; }

        public string ContentTitle { get; set; }

        public string ContentDescription { get; set; }

        public Message(string to, string subject, string communityName, 
            string content, string description)
        {
            To = new MailboxAddress("email", to);
            Subject = subject;
            CommunityName = communityName;
            ContentTitle = content;
            ContentDescription = description;
        }
    }
}

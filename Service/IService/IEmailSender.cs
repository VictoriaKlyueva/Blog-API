using BackendLaboratory.Data.Mailing;

namespace BackendLaboratory.Service.IService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);

        Task SendEmailAsync(Message message);
    }
}

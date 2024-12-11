using BackendLaboratory.Data.Mailing;

namespace BackendLaboratory.Service.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}

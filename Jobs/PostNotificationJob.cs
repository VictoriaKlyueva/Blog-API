using BackendLaboratory.Data;
using BackendLaboratory.Data.Mailing;
using BackendLaboratory.Service.IService;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace BackendLaboratory.Quartz
{
    public class PostNotificationJob : IJob
    {
        private IEmailService _emailService;
        private AppDBContext _db;

        public PostNotificationJob(IEmailService emailSenderService, AppDBContext context)
        {
            _emailService = emailSenderService;
            _db = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var newEmails = await _db.PostsUsers
                .Where(c => c.EmailStatus == false)
                .ToListAsync();

            foreach (var postUser in newEmails)
            {
                var subscriber = _db.Users.FirstOrDefault(u => u.Id == postUser.UserId);
                var post = _db.Posts.FirstOrDefault(p => p.Id == postUser.PostId);

                if (subscriber == null || post == null)
                {
                    _db.PostsUsers.Remove(postUser);

                }
                else
                {
                    var community = _db.Communities.FirstOrDefault(c => c.Id == post.CommunityId);

                    if (community == null)
                    {
                        _db.PostsUsers.Remove(postUser);
                    }
                    else
                    {
                        try
                        {
                            await _emailService.SendEmailAsync(
                            new Message(subscriber.Email, AppConstants.EmailTitle, community.Name,
                                post.Title, post.Description)
                            );
                            postUser.EmailStatus = true;
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка отправки email: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}

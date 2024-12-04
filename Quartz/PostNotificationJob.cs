using BackendLaboratory.Data;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
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
            var communitiesWithNewPosts = await _db.Communities
                .Where(c => _db.Posts
                    .Where(p =>  p.CommunityId == c.Id)
                        .Any(p => p.CreateTime > DateTime.UtcNow.AddMinutes(-3)))
                .ToListAsync();

            foreach (var community in communitiesWithNewPosts)
            {
                var subscribers = await _db.CommunityUsers
                    .Where(cu => cu.CommunityId == community.Id)
                    .Select(cu => cu.User)
                    .ToListAsync();

                foreach (var subscriber in subscribers)
                {
                    var newPosts = await _db.Posts
                        .Where(p => p.CommunityId == community.Id && p.CreateTime > DateTime.UtcNow.AddMinutes(-3))
                        .ToListAsync();

                    foreach (var post in newPosts)
                    {
                        await _emailService.SendAsync(subscriber.Email, AppConstants.EmailTitle, post.Title);
                    }
                }
            }
        }
    }
}

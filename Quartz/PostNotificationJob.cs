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

            Console.WriteLine("Джоба вызвалась");

            var communitiesWithNewPosts = await _db.Communities
                .Where(c => _db.Posts
                    .Where(p =>  p.CommunityId == c.Id)
                        .Any(p => p.CreateTime > DateTime.UtcNow.AddMinutes(-3)))
                .ToListAsync();

            Console.WriteLine("Комьюнити с новыми постами:");
            Console.WriteLine(communitiesWithNewPosts.Count());
            if (communitiesWithNewPosts.Count() > 0)
            {
                Console.WriteLine(communitiesWithNewPosts[0].Name);
            }

            foreach (var community in communitiesWithNewPosts)
            {
                var subscribers = await _db.CommunityUsers
                    .Where(cu => cu.CommunityId == community.Id)
                    .Select(cu => cu.User)
                    .ToListAsync();

                Console.WriteLine("Подписчики:");
                Console.WriteLine(subscribers.Count());
                foreach (var subscriber in subscribers)
                {
                    Console.WriteLine(subscriber.FullName);
                }

                foreach (var subscriber in subscribers)
                {
                    var newPost = await _db.Posts
                        .FirstOrDefaultAsync(p => p.CommunityId == community.Id && p.CreateTime > DateTime.UtcNow.AddMinutes(-3));

                    if (newPost != null)
                    {
                        Console.WriteLine("Пост:");
                        Console.WriteLine(newPost.Id.ToString(), newPost.Title);

                        await _emailService.SendEmailAsync(new Message(subscriber.Email, AppConstants.EmailTitle, newPost.Title));
                    }
                }
            }
        }
    }
}

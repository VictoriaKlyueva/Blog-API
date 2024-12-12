using BackendLaboratory.Data;
using BackendLaboratory.Util.Token;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace BackendLaboratory.Jobs
{
    public class CleanBlacklistTokensJob : IJob
    {
        private AppDBContext _db;
        private readonly TokenHelper _tokenHelper;

        public CleanBlacklistTokensJob(AppDBContext context, IConfiguration configuration)
        {
            _db = context;
            _tokenHelper = new TokenHelper(configuration);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var allTokens = await _db.BlackTokens.ToListAsync();

            var expiredTokens = allTokens
                .Where(bl => _tokenHelper.IsTokenExpired(bl.Blacktoken))
                .ToList();

            foreach (var token in expiredTokens)
            {
                _db.BlackTokens.Remove(token);
            }

            await _db.SaveChangesAsync();
        }
    }
}

using BackendLaboratory.Data.Database;
using BackendLaboratory.Service.IService;
using BackendLaboratory.Util.Token;
using Microsoft.EntityFrameworkCore;
using Quartz;
using StackExchange.Redis;

namespace BackendLaboratory.Jobs
{
    public class CleanBlacklistTokensJob : IJob
    {
        private readonly IConnectionMultiplexer _redis;

        public CleanBlacklistTokensJob(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var db = _redis.GetDatabase();

            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());

            var keys = server.Keys(pattern: $"{AppConstants.Blacklisted}:*");

            var expiredTokens = new List<RedisKey>();

            foreach (var key in keys)
            {
                var ttl = await db.KeyTimeToLiveAsync(key);
                if (ttl.HasValue && ttl.Value.TotalSeconds <= 0)
                {
                    expiredTokens.Add(key);
                }
            }

            if (expiredTokens.Any())
            {
                await db.KeyDeleteAsync(expiredTokens.ToArray());
            }
        }
    }
}

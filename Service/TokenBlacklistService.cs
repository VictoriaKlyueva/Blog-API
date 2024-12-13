using BackendLaboratory.Service.IService;
using StackExchange.Redis;

namespace BackendLaboratory.Service
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IConnectionMultiplexer _redis;

        public TokenBlacklistService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task AddTokenToBlacklistAsync(string token)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(token, AppConstants.Blacklisted, TimeSpan.FromDays(AppConstants.TokenExpiration));
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(token) != RedisValue.Null;
        }
    }
}

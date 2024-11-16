using BackendLaboratory.Repository.IRepository;
using System.Collections.Concurrent;

namespace BackendLaboratory.Repository
{
    public class TokenBlacklistRepository : ITokenBlacklistRepository
    {
        private static readonly ConcurrentDictionary<string, bool> _blacklist = new ConcurrentDictionary<string, bool>();

        public async Task AddToBlacklist(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _blacklist[token] = true;
            }

            await Task.CompletedTask;
        }

        public async Task<bool> IsBlacklisted(string token)
        {
            return await Task.FromResult(_blacklist.ContainsKey(token));
        }
    }
}

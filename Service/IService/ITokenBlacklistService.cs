namespace BackendLaboratory.Service.IService
{
    public interface ITokenBlacklistService
    {
        public Task AddTokenToBlacklistAsync(string token);

        public Task<bool> IsTokenBlacklistedAsync(string token);
    }
}

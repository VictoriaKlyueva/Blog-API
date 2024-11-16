namespace BackendLaboratory.Repository.IRepository
{
    public interface ITokenBlacklistRepository
    {
        Task AddToBlacklist(string token);
        Task<bool> IsBlacklisted(string token);
    }

}

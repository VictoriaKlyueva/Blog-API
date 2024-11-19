using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommunityRepository
    {
        Task<List<CommunityDto>> GetCommunities();

        Task SubscribeToCommunity(string token, string communityId);

        Task UnsubscribeFromCommunity(string token, string communityId);
    }
}

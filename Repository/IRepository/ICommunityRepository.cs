using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommunityRepository
    {
        Task<List<CommunityDto>> GetCommunities();

        Task<List<CommunityUserDto>> GetUserCommunities(string token);

        Task SubscribeToCommunity(string token, string communityId);

        Task UnsubscribeFromCommunity(string token, string communityId);
    }
}

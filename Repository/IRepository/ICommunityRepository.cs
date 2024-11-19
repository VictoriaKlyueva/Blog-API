using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommunityRepository
    {
        Task<List<CommunityDto>> GetCommunities();

        Task SubstribeToCommunity(string token, string communityId);

        Task UnsubstribeFromCommunity(string token, string communityId);
    }
}

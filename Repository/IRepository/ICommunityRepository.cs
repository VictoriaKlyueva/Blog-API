using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommunityRepository
    {
        Task<List<CommunityDto>> GetCommunities();
    }
}

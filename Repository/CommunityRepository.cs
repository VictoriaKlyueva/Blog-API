using BackendLaboratory.Data.DTO;
using BackendLaboratory.Repository.IRepository;

namespace BackendLaboratory.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        public async Task<List<CommunityDto>> GetCommunities()
        {
            await Task.CompletedTask;
            return new List<CommunityDto>();
        }
    }
}

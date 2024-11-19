using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly AppDBContext _db;

        public CommunityRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<CommunityDto>> GetCommunities()
        {
            var communities = await _db.Communities.ToListAsync();

            return communities.Select(community => new CommunityDto
            {
                Id = community.Id,
                CreateTime = community.CreateTime,
                Name = community.Name,
                Description = community.Description,
                IsClosed = community.IsClosed,
                SubscribersCount = community.SubscribersCount
            }).ToList();
        }
    }
}

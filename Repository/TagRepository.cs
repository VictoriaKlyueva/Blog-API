using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackendLaboratory.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDBContext _db;

        public TagRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<TagDto>> GetTags()
        {
            return await _db.Tags.ToListAsync();
        }
    }
}

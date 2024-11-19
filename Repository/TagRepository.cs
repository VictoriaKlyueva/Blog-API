using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Migrations;
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
            var tags = await _db.Tags.ToListAsync();
            return tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                CreateTime = tag.CreateTime,
                Name = tag.Name
            }).ToList();
        }
    }
}

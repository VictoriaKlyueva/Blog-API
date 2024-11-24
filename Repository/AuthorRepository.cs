using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDBContext _db;

        public AuthorRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<AuthorDto>> GetAuthors()
        {
            var authors = await _db.Users
                .Where(user => _db.Posts.Any(post => post.AuthorId == user.Id))
                .Select(user => new AuthorDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    BirthDate = user.BirthDate,
                    gender = user.Gender,
                    Posts = _db.Posts.Where(post => post.AuthorId == user.Id).Count(),
                    Likes = user.Posts
                        .Where(post => user.Id == post.AuthorId)
                        .Sum(post => post.Likes),
                    Created = user.CreateTime
                })
                .ToListAsync();

            return authors;
        }
    }
}

using BackendLaboratory.Constants;
using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using BackendLaboratory.Util.Token;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDBContext _db;
        private readonly TokenHelper _tokenHelper;

        public PostRepository(AppDBContext db, IConfiguration configuration)
        {
            _db = db;
            _tokenHelper = new TokenHelper(configuration);
        }

        public async Task CreatePost(string token, CreatePostDto createPostDto)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            Post post = new()
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                Title = createPostDto.Title,
                Description = createPostDto.Description,
                ReadingTime = createPostDto.ReadingTime,
                Image = createPostDto.Image,
                AuthorId = new Guid(userId),
                CommunityId = null,
                AddressId = null, // Исправить на адрес
                Likes = 0,
                CommentsCount = 0
            };

            _db.Posts.Add(post);

            await _db.SaveChangesAsync();
        }
    }
}

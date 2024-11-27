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
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDBContext _db;
        private readonly TokenHelper _tokenHelper;

        public CommentRepository(AppDBContext db, IConfiguration configuration)
        {
            _db = db;
            _tokenHelper = new TokenHelper(configuration);
        }

        public async Task AddComment(string postId, string token,
            CreateCommentDto createCommentDto)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var post = await _db.Posts
                .FirstOrDefaultAsync(post => post.Id.ToString() == postId);

            if (post == null) { throw new NotFoundException(ErrorMessages.PostNotFound); }

            if (post.CommunityId != null)
            {
                var postCommunity = await _db.Communities
                .FirstOrDefaultAsync(c => c.Id == post.CommunityId);

                if (postCommunity == null)
                {
                    throw new NotFoundException(ErrorMessages.CommunityNotFound);
                }

                if (postCommunity.IsClosed &&
                    _db.CommunityUsers
                        .FirstOrDefault(cu =>
                            cu.CommunityId == postCommunity.Id &&
                            cu.UserId.ToString() == userId) == null)
                {
                    throw new ForbiddenException(ErrorMessages.CommunityPostsForbidden);
                }
            }

            Comment comment = new ()
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                Content = createCommentDto.Content,
                ModifiedDate = null,
                DeleteDate = null,
                ParentId = createCommentDto.ParentId,
                AuthorId = new Guid(userId),
                PostId = new Guid(postId)
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
        }
    }
}

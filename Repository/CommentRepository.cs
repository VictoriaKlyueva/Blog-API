using BackendLaboratory.Constants;
using BackendLaboratory.Data.Database;
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

        public async Task<List<CommentDto>> GetCommentsTree(string parentId, string? token)
        {
            string? userId = _tokenHelper.GetIdFromToken(token);
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            var root = await _db.Comments
                .Include(c => c.ChildComments)
                .FirstOrDefaultAsync(c => c.Id.ToString() == parentId);

            if (root == null) 
            { 
                throw new NotFoundException(ErrorMessages.CommentNotFound); 
            }
            if (root.ParentId != null)
            {
                throw new BadRequestException(ErrorMessages.CommentIsNotRoot);
            }

            var commentTree = GetCommentChilds(root.Id);
            commentTree.RemoveAt(0);

            return commentTree;
        }

        private List<CommentDto> GetCommentChilds(Guid commentId)
        {
            var comment = _db.Comments
                .Include(c => c.ChildComments)
                .FirstOrDefault(c => c.Id == commentId);

            if (comment == null) 
            { 
                throw new NotFoundException(ErrorMessages.ConcreteCommentNotFound(commentId.ToString()));
            }

            CommentDto commentDto = new CommentDto
            {
                Id = comment.Id.ToString(),
                CreateTime = comment.CreateTime,
                Content = comment.Content,
                ModifiedDate = comment.ModifiedDate,
                DeleteDate = comment.DeleteDate,
                AuthorId = comment.AuthorId.ToString(),
                Author = AppConstants.EmptyString,
                SubComments = comment.ChildComments.Count
            };

            var commentAuthor = _db.Users.FirstOrDefault(u => u.Id == comment.AuthorId);
            if (commentAuthor == null) { throw new NotFoundException(ErrorMessages.AuthorNotFound); };
            commentDto.Author = commentAuthor.FullName;
            
            var commentTree = new List<CommentDto> { commentDto };

            foreach (var subComment in comment.ChildComments)
            {
                var subCommentTree = GetCommentChilds(subComment.Id);
                commentTree.AddRange(subCommentTree);
            }

            return commentTree;
        }

        public async Task AddComment(string postId, string token,
            CreateCommentDto createCommentDto)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            if (string.IsNullOrWhiteSpace(createCommentDto.Content))
            {
                throw new BadRequestException(ErrorMessages.IncorrectCommentContent);
            }

            if (createCommentDto.ParentId != null)
            {
                var parentComment = _db.Comments
                .FirstOrDefault(c => c.Id == createCommentDto.ParentId);
                if (parentComment == null)
                {
                    throw new NotFoundException(ErrorMessages.ParentCommentNotFound);
                }
            }
            
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
            post.CommentsCount += 1;

            await _db.SaveChangesAsync();
        }

        public async Task EditComment(string commentId, string token, 
            UpdateCommentDto updateCommentDto)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            if (string.IsNullOrWhiteSpace(updateCommentDto.Content))
            {
                throw new BadRequestException(ErrorMessages.IncorrectCommentContent);
            }

            var comment = await _db.Comments
                .FirstOrDefaultAsync(c => c.Id.ToString() == commentId);
            if (comment == null) { throw new NotFoundException(ErrorMessages.CommentNotFound); }

            if (comment.AuthorId.ToString() != userId)
            {
                throw new ForbiddenException(ErrorMessages.CommentForbidden);
            }

            comment.Content = updateCommentDto.Content;
            comment.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteComment(string commentId, string token)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var comment = await _db.Comments
                .FirstOrDefaultAsync(c => c.Id.ToString() == commentId);
            if (comment == null) { throw new NotFoundException(ErrorMessages.CommentNotFound); }

            var post = await _db.Posts
                .FirstOrDefaultAsync(post => post.Id == comment.PostId);

            if (post == null) { throw new NotFoundException(ErrorMessages.PostNotFound); }

            if (comment.AuthorId.ToString() != userId)
            {
                var communityUser = await _db.CommunityUsers
                    .FirstOrDefaultAsync(cu => 
                        cu.UserId.ToString() == userId && 
                        cu.CommunityId == post.CommunityId);
                if (communityUser == null)
                {
                    throw new ForbiddenException(ErrorMessages.CommentForbidden);
                }
                if (communityUser.Role == Data.Entities.Enums.CommunityRole.Substriber)
                {
                    throw new ForbiddenException(ErrorMessages.CommentForbidden);
                }
            }

            comment.Content = AppConstants.EmptyString;
            comment.DeleteDate = DateTime.UtcNow;
            post.CommentsCount -= 1;

            await _db.SaveChangesAsync();
        }
    }
}

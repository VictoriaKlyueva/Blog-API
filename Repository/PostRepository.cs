using BackendLaboratory.Constants;
using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using BackendLaboratory.Util.Token;
using BackendLaboratory.Util.Validators;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PostPagedListDto> GetPosts(List<Guid>? tags, string? author, 
            int? min, int? max, PostSorting? sorting, bool onlyMyCommunities,
            int page, int size, string token)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            QueryValidation.IsPostDataValid(page, size, min, max);

            var posts = _db.Posts.AsQueryable();
            posts = ApplyFilters(posts, tags, author, min, max, onlyMyCommunities, user);
        }

        private IQueryable<Post> ApplyFilters(IQueryable<Post> posts, List<Guid>? tags, 
            string? author, int? minReadingTime, int? maxReadingTime, 
            bool onlyMyCommunities, User? user)
        {
            if (tags != null && tags.Any())
            {
                foreach (var tagId in tags)
                {
                    var tag = _db.Tags.FirstOrDefault(t => t.Id == tagId);

                    if (tag == null)
                    {
                        throw new BadRequestException(
                            ErrorMessages.ConcreteTagNotFound(tagId.ToString())
                        );
                    }
                }

                var postsWithTags = _db.PostTags
                    .Where(pt => tags.Any(t => t == pt.TagId))
                    .Select(pt => pt.PostId)
                    .Distinct();

                posts = posts.Where(post => postsWithTags.Contains(post.Id));
            }

            if (!string.IsNullOrEmpty(author))
            {
                var authorId = _db.Users
                   .Where(u => u.FullName == author)
                   .Select(u => u.Id)
                   .FirstOrDefault();

                posts = posts.Where(post => post.AuthorId == authorId);
            }

            if (minReadingTime != null)
            {
                posts = posts.Where(p => p.ReadingTime >= minReadingTime.Value);
            }

            if (maxReadingTime != null)
            {
                posts = posts.Where(p => p.ReadingTime <= maxReadingTime.Value);
            }

            if (user != null)
            {
                // Посты только тех закрытых комьюнити в которых состоит юзер
                posts = posts.Where(post =>
                    post.CommunityId == null || _db.Communities.Any(community =>
                        community.Id == post.CommunityId &&
                        (community.IsClosed && _db.CommunityUsers
                            .Any(c => c.UserId == user.Id && c.CommunityId == community.Id) ||
                            !community.IsClosed)
                    ));

            }

            if (onlyMyCommunities)
            {
                if (user == null)
                {
                    throw new UnauthorizedException(ErrorMessages.ProfileNotFound);
                }

                posts = posts
                    .Where(p => _db.CommunityUsers
                        .Any(c => c.UserId == user.Id && c.CommunityId == p.CommunityId));
            }

            return posts;
        }

        public async Task CreatePost(string token, CreatePostDto createPostDto)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            if (createPostDto.Tags != null && createPostDto.Tags.Any())
            {
                foreach (var tagId in createPostDto.Tags.Distinct())
                {
                    var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                    if (tag == null) { throw new NotFoundException(ErrorMessages.TagNotFound); }
                }
            }

            Post post = new()
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
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

            if (createPostDto.Tags != null && createPostDto.Tags.Any())
            {
                foreach (var tagId in createPostDto.Tags.Distinct())
                {
                    var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                    if (tag == null) { throw new NotFoundException(ErrorMessages.TagNotFound); }

                    post.Tags.Add(tag);
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task<PostFullDto> GetPostInfo(string token, string postId)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var post = await _db.Posts.FirstOrDefaultAsync(c => c.Id.ToString() == postId);
            if (post == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

            // Проверка на доступ
            if (post.CommunityId != null)
            {
                var community = await _db.Communities
                    .FirstOrDefaultAsync(c => c.Id == post.CommunityId);
                if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

                // Если сообщество закрытое, то пользователь должен быть подписан
                if (community.IsClosed)
                {
                    var communityUser = await _db.CommunityUsers
                        .FirstOrDefaultAsync(
                            cu => cu.UserId.ToString() == userId && 
                            cu.CommunityId == community.Id
                        );

                    if (communityUser == null)
                    {
                        throw new ForbiddenException(ErrorMessages.PostForbidden);
                    }
                }
            }

            PostFullDto postFullDto = new()
            {
                Id = post.Id,
                CreateTime = post.CreateTime,
                Title = post.Title,
                Description = post.Description,
                ReadingTime = post.ReadingTime,
                Image = post.Image,
                AuthorId = post.AuthorId,
                Author = AppConstants.EmptyString,
                CommunityId = post.CommunityId,
                CommunityName = null,
                AddressId = post.AddressId,
                Likes = post.Likes,
                HasLike = false,
                CommentsCount = post.CommentsCount,
                Tags = new List<TagDto>(),
                Comments = new List<CommentDto>() // Заменить на комментарии
            };

            var author = await _db.Users.FirstOrDefaultAsync(a => a.Id == post.AuthorId);
            if (author == null) { throw new NotFoundException(ErrorMessages.AuthorNotFound); }
            postFullDto.Author = author.FullName;

            if (post.CommunityId != null)
            {
                var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id == post.CommunityId);
                if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }
                postFullDto.CommunityName = community.Name;
            }

            var likeLink = await _db.LikesLink
                .FirstOrDefaultAsync(
                    cu => cu.UserId.ToString() == userId && 
                    cu.PostId == post.Id
                );
            if (likeLink != null)
            {
                postFullDto.HasLike = true;
            }

            var postTags = await _db.PostTags
                .Where(pt => pt.PostId == post.Id)
                .Include(pt => pt.Tag)
                .ToListAsync();

            if (postTags != null)
            {
                postFullDto.Tags = postTags.Select(pt => new TagDto
                {
                    Id = pt.Tag.Id,
                    CreateTime = pt.Tag.CreateTime,
                    Name = pt.Tag.Name
                }).ToList();
            }

            return postFullDto;
        }

        public async Task AddLike(string token, string postId)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var post = await _db.Posts.FirstOrDefaultAsync(c => c.Id.ToString() == postId);
            if (post == null)
            {
                throw new NotFoundException(ErrorMessages.CommunityNotFound);
            }

            // Проверка на доступ
            if (post.CommunityId != null)
            {
                var community = await _db.Communities
                    .FirstOrDefaultAsync(c => c.Id == post.CommunityId);
                if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

                // Если сообщество закрытое, то пользователь должен быть подписан
                if (community.IsClosed)
                {
                    var communityUser = await _db.CommunityUsers
                        .FirstOrDefaultAsync(
                            cu => cu.UserId.ToString() == userId &&
                            cu.CommunityId == community.Id
                        );

                    if (communityUser == null)
                    {
                        throw new ForbiddenException(ErrorMessages.PostForbidden);
                    }
                }
            }

            var existingLike = await _db.LikesLink
                .FirstOrDefaultAsync(
                    cu => cu.UserId.ToString() == userId && 
                    cu.PostId.ToString() == postId
                );
            if (existingLike != null)
            {
                throw new BadRequestException(ErrorMessages.LikeIsAlreadyAdded);
            }

            user.LikesLink.Add(new Like { 
                Post = post
            });
            post.Likes += 1;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteLike(string token, string postId)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var post = await _db.Posts.FirstOrDefaultAsync(c => c.Id.ToString() == postId);
            if (post == null)
            {
                throw new NotFoundException(ErrorMessages.CommunityNotFound);
            }

            // Проверка на доступ
            if (post.CommunityId != null)
            {
                var community = await _db.Communities
                    .FirstOrDefaultAsync(c => c.Id == post.CommunityId);
                if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

                // Если сообщество закрытое, то пользователь должен быть подписан
                if (community.IsClosed)
                {
                    var communityUser = await _db.CommunityUsers
                        .FirstOrDefaultAsync(
                            cu => cu.UserId.ToString() == userId &&
                            cu.CommunityId == community.Id
                        );

                    if (communityUser == null)
                    {
                        throw new ForbiddenException(ErrorMessages.PostForbidden);
                    }
                }
            }

            var like = await _db.LikesLink
                .FirstOrDefaultAsync(
                    cu => cu.UserId.ToString() == userId &&
                    cu.PostId.ToString() == postId
                );
            if (like == null)
            {
                throw new BadRequestException(ErrorMessages.LikeHasntBeenAdded);
            }

            user.LikesLink.Remove(like);
            post.Likes -= 1;

            await _db.SaveChangesAsync();
        }
    }
}

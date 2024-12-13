using BackendLaboratory.Constants;
using BackendLaboratory.Data.Database;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using BackendLaboratory.Util.Token;
using BackendLaboratory.Util.Validators;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDBContext _db;
        private readonly GarContext _gar;
        private readonly TokenHelper _tokenHelper;

        public PostRepository(AppDBContext db, GarContext gar, IConfiguration configuration)
        {
            _db = db;
            _gar = gar;
            _tokenHelper = new TokenHelper(configuration);
        }

        public async Task<PostPagedListDto> GetPosts(List<Guid>? tags, string? author, 
            int? min, int? max, PostSorting? sorting, bool onlyMyCommunities,
            int page, int size, string? token)
        {
            string? userId = _tokenHelper.GetIdFromToken(token);
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            QueryValidation.IsPostDataValid(page, size, min, max);

            var posts = _db.Posts.AsQueryable();
            posts = ApplyFilters(posts, tags, author, min, max, 
                sorting, onlyMyCommunities, user);

            var pagesCount = Math.Max((int)Math.Ceiling((double)posts.Count() / size), 1);
            if (pagesCount < page) { throw new NotFoundException(ErrorMessages.PageNotFound); }

            var paginatedPosts = ApplyPagination(posts, page, size);

            List<PostDto> paginatedPostsDto = await paginatedPosts
                .Select(post => new PostDto
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
                    Tags = new List<TagDto>()
                })
                .ToListAsync();

            foreach (var post in paginatedPostsDto)
            {
                User? postAuthor = _db.Users.FirstOrDefault(a => a.Id == post.AuthorId);
                if (postAuthor == null) { throw new NotFoundException(ErrorMessages.AuthorNotFound); }
                post.Author = postAuthor.FullName;

                if (post.CommunityId != null)
                {
                    var community = await _db.Communities
                        .FirstOrDefaultAsync(c => c.Id == post.CommunityId);
                    if (community == null) 
                    { 
                        throw new NotFoundException(ErrorMessages.CommunityNotFound); 
                    }
                    post.CommunityName = community.Name;
                }

                var likeLink = await _db.LikesLink
                .FirstOrDefaultAsync(
                    cu => cu.UserId.ToString() == userId &&
                    cu.PostId == post.Id
                );
                if (likeLink != null)
                {
                    post.HasLike = true;
                }

                var postTags = await _db.PostTags
                    .Where(pt => pt.PostId == post.Id)
                    .Include(pt => pt.Tag)
                    .ToListAsync();

                if (postTags != null)
                {
                    post.Tags = postTags.Select(pt => new TagDto
                    {
                        Id = pt.Tag.Id,
                        CreateTime = pt.Tag.CreateTime,
                        Name = pt.Tag.Name
                    }).ToList();
                }
            }

            PageInfoModel pageInfoModel = new PageInfoModel
            {
                Count = pagesCount,
                Size = size,
                Current = page
            };

            return new PostPagedListDto
            {
                Posts = paginatedPostsDto,
                Pagination = pageInfoModel
            };
        }

        private IQueryable<Post> ApplyFilters(IQueryable<Post> posts, List<Guid>? tags, 
            string? author, int? min, int? max, PostSorting? sorting,
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

            if (min != null)
            {
                posts = posts.Where(p => p.ReadingTime >= min.Value);
            }

            if (max != null)
            {
                posts = posts.Where(p => p.ReadingTime <= max.Value);
            }

            if (user != null)
            {
                // Посты только тех закрытых комьюнити в которых состоит юзер
                posts = posts.Where(post =>
                    post.CommunityId == null || _db.Communities.Any(community =>
                        community.Id == post.CommunityId &&
                        (community.IsClosed && _db.CommunityUsers
                            .Any(cu => cu.UserId == user.Id && cu.CommunityId == community.Id) ||
                        !community.IsClosed)
                    ));
            }
            else
            {
                // Посты без закрытых комьюнити
                posts = posts.Where(post =>
                    post.CommunityId == null || _db.Communities.Any(community =>
                        community.Id == post.CommunityId && !community.IsClosed
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

            posts = sorting switch
            {
                PostSorting.CreateAsc => posts.OrderBy(p => p.CreateTime),
                PostSorting.CreateDesс => posts.OrderByDescending(p => p.CreateTime),
                PostSorting.LikeAsc => posts.OrderBy(p => p.Likes),
                PostSorting.LikeDesc => posts.OrderByDescending(p => p.Likes),
                _ => posts,
            };

            return posts;
        }

        private IQueryable<Post> ApplyPagination(IQueryable<Post> posts, int page, int size)
        {
            return posts
                .Skip((page - 1) * size)
                .Take(size);
        }

        public async Task CreatePost(string? token, CreatePostDto createPostDto)
        {

            PostsValidator.IsPostDataValid(createPostDto);

            var userId = _tokenHelper.GetIdFromToken(token);
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

            Guid guid;
            if (!Guid.TryParse(createPostDto.AddressId, out guid))
            {
                throw new BadRequestException(ErrorMessages.IncorrectGuid);
            }

            var address = await _gar.AsAddrObjs
                .FirstOrDefaultAsync(a => a.ObjectGuid.ToString() == createPostDto.AddressId);

            if (address == null)
            {
                var house = await _gar.AsHouses
                    .FirstOrDefaultAsync(h => h.ObjectGuid.ToString() == createPostDto.AddressId);

                if (house == null)
                {
                    throw new NotFoundException(ErrorMessages.AddressNotFound);
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
                AuthorId = new Guid(userId!),
                CommunityId = null,
                AddressId = new Guid(createPostDto.AddressId),
                Likes = 0,
                CommentsCount = 0
            };

            _db.Posts.Add(post);

            if (createPostDto.Tags != null && createPostDto.Tags.Any())
            {
                foreach (var tagId in createPostDto.Tags.Distinct())
                {
                    var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                    if (tag == null) { throw new NotFoundException(ErrorMessages.TagNotFound); }

                    post.Tags.Add(tag);
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task<PostFullDto> GetPostInfo(string? token, string postId)
        {
            string? userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

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
                    if (user == null)
                    { 
                        throw new ForbiddenException(ErrorMessages.PostForbidden);
                    }

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
                Comments = new List<CommentDto>()
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

            var comments = _db.Comments
                .Where(c => c.PostId.ToString() == postId && c.ParentId == null)
                .Select(c => new CommentDto
                {
                    Id = c.Id.ToString(),
                    CreateTime = c.CreateTime,
                    Content = c.Content,
                    ModifiedDate = c.ModifiedDate,
                    DeleteDate = c.DeleteDate,
                    AuthorId = c.AuthorId.ToString(),
                    Author = AppConstants.EmptyString,
                    SubComments = c.ChildComments.Count
                })
                .ToList();

            for (int i = 0; i < comments.Count; i++ )
            {
                var commentAuthor = _db.Users
                    .FirstOrDefault(u => u.Id.ToString() == comments[i].AuthorId);
                if (commentAuthor == null)
                {
                    throw new NotFoundException(ErrorMessages.AuthorNotFound);
                }

                comments[i].Author = commentAuthor.FullName;
            }

            postFullDto.Comments = comments;
            return postFullDto;
        }

        public async Task AddLike(string? token, string postId)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
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

        public async Task DeleteLike(string? token, string postId)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
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

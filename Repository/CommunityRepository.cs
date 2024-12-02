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

namespace BackendLaboratory.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly AppDBContext _db;
        private readonly TokenHelper _tokenHelper;

        public CommunityRepository(AppDBContext db, IConfiguration configuration)
        {
            _db = db;
            _tokenHelper = new TokenHelper(configuration);
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

        public async Task<List<CommunityUserDto>> GetUserCommunities(string token)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var communityUsers = await _db.CommunityUsers
                .Where(p => p.UserId.ToString() == userId)
                .Select(communityUser => new CommunityUserDto
                {
                    UserId = communityUser.UserId,
                    CommunityId = communityUser.CommunityId,
                    Role = communityUser.Role
                })
                .ToListAsync();

            if (communityUsers.Count == 0) { throw new NotFoundException(ErrorMessages.CommunitiesNotFound); }

            return communityUsers;
        }

        public async Task<CommunityFullDto> GetCommunityInfo(string communityId)
        {
            var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id.ToString() == communityId);
            if (community == null)
            {
                throw new NotFoundException(ErrorMessages.CommunityNotFound);
            }

            var administrators = await _db.CommunityUsers
                .Where(p => p.CommunityId == community.Id && p.Role == CommunityRole.Administrator)
                .Include(cu => cu.User)
                .ToListAsync();

            var communityInfo = new CommunityFullDto
            {
                Id = community.Id,
                CreateTime = community.CreateTime,
                Name = community.Name,
                Description = community.Description,
                IsClosed = community.IsClosed,
                SubscribersCount = community.SubscribersCount,
                Administrators = administrators.Select(cu => new UserDto
                {
                    Id = cu.User.Id,
                    CreateTime = cu.User.CreateTime,
                    FullName = cu.User.FullName,
                    BirthDate = cu.User.BirthDate,
                    Gender = cu.User.Gender,
                    Email = cu.User.Email,
                    PhoneNumber = cu.User.PhoneNumber
                }).ToList()
            };

            return communityInfo;
        }

        public async Task<PostPagedListDto> GetCommunityPosts(List<Guid>? tags,
            PostSorting? sorting, int page, int size, string? token, string id)
        {
            var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id.ToString() == id);
            if (community == null)
            {
                throw new NotFoundException(ErrorMessages.CommunityNotFound);
            }

            string? userId = _tokenHelper.GetIdFromToken(token);
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (community.IsClosed &&
                (userId == null || 
                _db.CommunityUsers
                    .FirstOrDefault(cu =>
                        cu.CommunityId.ToString() == id && cu.UserId.ToString() == userId) != null))
            {
                throw new ForbiddenException(ErrorMessages.CommunityPostsForbidden);
            }

            QueryValidation.IsCommunityDataValid(page, size);

            var posts = _db.Posts
                .Where(post => post.CommunityId.ToString() == id)
                .AsQueryable();

            posts = ApplyFilters(posts, tags, sorting, user);

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
                    CommunityName = community.Name,
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
                if (postAuthor == null) 
                { 
                    throw new NotFoundException(ErrorMessages.AuthorNotFound); 
                }

                post.Author = postAuthor.FullName;

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
            PostSorting? sorting, User? user)
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

        public async Task<CommunityRole?> GetCommunityRole(string token, string communityId)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            { 
                throw new UnauthorizedException(ErrorMessages.ProfileNotFound); 
            }

            var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id.ToString() == communityId);
            if (community == null)
            {
                throw new NotFoundException(ErrorMessages.CommunityNotFound);
            }

            var communityUser = await _db.CommunityUsers
                .FirstOrDefaultAsync(cu => 
                    cu.UserId.ToString() == userId && 
                    cu.CommunityId.ToString() == communityId
                );
            if (communityUser == null)
            { 
                return null;
            }
            
            return communityUser.Role;
        }

        public async Task CreateCommunityPost(string token, string communityId, CreatePostDto createPostDto)
        {
            // Check authorization
            string userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            // Check is community found
            var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id.ToString() == communityId);
            if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

            // Check user can make post
            var communityUser = await _db.CommunityUsers
                .FirstOrDefaultAsync(cu => cu.UserId.ToString() == userId && cu.CommunityId.ToString() == communityId);
            if (communityUser == null)
            {
                throw new ForbiddenException(ErrorMessages.UserIsNotSubstribed);
            }
            if (communityUser.Role == CommunityRole.Substriber)
            {
                throw new ForbiddenException(ErrorMessages.UserCantMakePost);
            }

            // Check tags
            if (createPostDto.Tags != null && createPostDto.Tags.Any())
            {
                foreach (var tagId in createPostDto.Tags.Distinct())
                {
                    var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                    if (tag == null) { throw new NotFoundException(ErrorMessages.TagNotFound); }
                }
            }

            Data.Entities.Post post = new()
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                Title = createPostDto.Title,
                Description = createPostDto.Description,
                ReadingTime = createPostDto.ReadingTime,
                Image = createPostDto.Image,
                AuthorId = new Guid(userId),
                CommunityId = new Guid(communityId),
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

        public async Task SubscribeToCommunity(string token, string communityId)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id.ToString() == communityId);
            if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

            var existingCommunityUser = await _db.CommunityUsers
                .FirstOrDefaultAsync(cu => cu.UserId.ToString() == userId && cu.CommunityId.ToString() == communityId);
            if (existingCommunityUser != null)
            {
                throw new BadRequestException(ErrorMessages.UserIsAlreadySubstribed);
            }

            user.CommunityUsers.Add(new CommunityUser { Community = community, Role = CommunityRole.Substriber });
            community.SubscribersCount += 1;

            await _db.SaveChangesAsync();
        }

        public async Task UnsubscribeFromCommunity(string token, string communityId)
        {
            var userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            var community = await _db.Communities.FirstOrDefaultAsync(c => c.Id.ToString() == communityId);
            if (community == null) { throw new NotFoundException(ErrorMessages.CommunityNotFound); }

            var communityUser = await _db.CommunityUsers
                .FirstOrDefaultAsync(cu => cu.UserId.ToString() == userId && cu.CommunityId.ToString() == communityId);
            if (communityUser == null)
            {
                throw new BadRequestException(ErrorMessages.UserIsNotSubstribed);
            }

            _db.CommunityUsers.Remove(communityUser);
            community.SubscribersCount -= 1;

            await _db.SaveChangesAsync();
        }
    }
}

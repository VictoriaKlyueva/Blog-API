﻿using BackendLaboratory.Constants;
using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;
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

        public Task CreateCommunityPost(string token, CreatePostDto createPostDto)
        {
            throw new NotImplementedException();
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
    }
}

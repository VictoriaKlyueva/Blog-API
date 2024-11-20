using BackendLaboratory.Constants;
using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;
using BackendLaboratory.Migrations;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using BackendLaboratory.Util.Token;
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


        public async Task SubscribeToCommunity(string token, string communityId)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
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
            await _db.SaveChangesAsync();
        }

        public async Task UnsubscribeFromCommunity(string token, string communityId)
        {
            string userId = _tokenHelper.GetIdFromToken(token);
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
            await _db.SaveChangesAsync();
        }
    }
}

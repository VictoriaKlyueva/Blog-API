using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities.Enums;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommunityRepository
    {
        Task<List<CommunityDto>> GetCommunities();

        Task<List<CommunityUserDto>> GetUserCommunities(string token);
        Task<CommunityFullDto> GetCommunityInfo(string communityId);

        Task<PostPagedListDto> GetCommunityPosts(List<Guid>? tags, 
            PostSorting? sorting, int page, int size, string? token, string id);

        Task<CommunityRole?> GetCommunityRole(string token, string communityId);

        Task CreateCommunityPost(string token, string communityId, CreatePostDto createPostDto);

        Task SubscribeToCommunity(string token, string communityId);

        Task UnsubscribeFromCommunity(string token, string communityId);
    }
}

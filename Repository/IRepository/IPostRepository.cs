using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities.Enums;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IPostRepository
    {
        Task<PostPagedListDto> GetPosts(List<Guid>? tags, string? author, int? min, 
            int? max, PostSorting? sorting, bool onlyMyCommunities, 
            int page, int size, string? token);

        Task CreatePost(string token, CreatePostDto createPostDto);

        Task<PostFullDto> GetPostInfo(string token, string postId);

        Task AddLike(string token, string postId);

        Task DeleteLike(string token, string postId);
    }
}

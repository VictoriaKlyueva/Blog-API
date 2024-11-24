using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IPostRepository
    {
        Task CreatePost(string token, CreatePostDto createPostDto);

        Task CreateCommunityPost(string token, CreatePostDto createPostDto);

        Task<PostFullDto> GetPostInfo(string token, string postId);

        Task AddLike(string token, string postId);

        Task DeleteLike(string token, string postId);
    }
}

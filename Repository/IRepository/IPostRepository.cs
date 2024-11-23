using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IPostRepository
    {
        Task<Post> CreatePost(string token, CreatePostDto createPostDto);
    }
}

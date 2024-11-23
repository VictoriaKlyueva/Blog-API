using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IPostRepository
    {
        Task CreatePost(string token, CreatePostDto createPostDto);
    }
}

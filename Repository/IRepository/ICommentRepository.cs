using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommentRepository
    {
        Task AddComment(string postId, string token, 
            CreateCommentDto createCommentDto);
    }
}

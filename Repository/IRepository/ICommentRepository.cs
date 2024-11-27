using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ICommentRepository
    {
        Task AddComment(string postId, string token, 
            CreateCommentDto createCommentDto);

        Task EditComment(string commentId, string token,
            UpdateCommentDto updateCommentDto);

        Task DeleteComment(string commentId, string token);
    }
}

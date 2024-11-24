using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IAuthorRepository
    {
        Task<List<AuthorDto>> GetAuthors();
    }
}

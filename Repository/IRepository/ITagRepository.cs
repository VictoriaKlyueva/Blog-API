using BackendLaboratory.Data.DTO;

namespace BackendLaboratory.Repository.IRepository
{
    public interface ITagRepository
    {
        Task<List<TagDto>> GetTags();
    }
}

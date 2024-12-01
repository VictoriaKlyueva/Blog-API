using BackendLaboratory.Data.Entities;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IAddressRepository
    {
        Task<List<SearchAddressModel>> SearchAdress(long? parentObjectId, string? query);

        Task<List<SearchAddressModel>> GetAddressChain(string? objectId);
    }
}

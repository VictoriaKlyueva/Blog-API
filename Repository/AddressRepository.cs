using BackendLaboratory.Constants;
using BackendLaboratory.Data;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util.Address;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly GarContext _db;

        public AddressRepository(GarContext db)
        {
            _db = db;
        }

        public async Task<List<SearchAddressModel>> SearchAdress(long? parentObjectId, string? query)
        {
            var result = new List<SearchAddressModel>();

            if (parentObjectId == null)
            {
                parentObjectId = 0;
            }
            else
            {
                var parentObject = await _db.AsAddrObjs
                    .FirstOrDefaultAsync(a => a.ObjectId == parentObjectId);

                if (parentObject == null)
                { 
                    throw new NotFoundException(ErrorMessages.AddressNotFound); 
                } 
            }

            var addresses = _db.AsAddrObjs
                .Where(x => x.Parentobjid == parentObjectId)
                .AsQueryable();

            if (!addresses.Any())
            {
                var houses = _db.AsHouses
                    .Where(x => x.Parentobjid == parentObjectId)
                    .AsQueryable();

                result = houses.Select(h => new SearchAddressModel
                {
                    ObjectId = h.ObjectId,
                    ObjectGuid = h.Objectguid,
                    Text = AddressHelper.GetHouseName(h),
                    ObjectLevel = ObjectLevel.Building,
                    ObjectLevelText = AddressHelper.GetAddressLevelName(10)
                }).ToList();
            }
            else
            {
                result = addresses.Select(a => new SearchAddressModel
                {
                    ObjectId = a.ObjectId,
                    ObjectGuid = a.ObjectGuid,
                    Text = AddressHelper.GetAddressName(a.Typename, a.Name),
                    ObjectLevel = AddressHelper.GetAddressLevel(Convert.ToInt32(a.Level)),
                    ObjectLevelText = AddressHelper.GetAddressLevelName(Convert.ToInt32(a.Level))
                }).ToList();
            }

            if (query != null)
            {
                result = result
                    .Where(r => 
                        r.Text != null && 
                        r.Text.ToLower().Contains(query.ToLower()))
                    .ToList();
            }

            return result;
        }

        public Task<List<SearchAddressModel>> GetAddressChain(string? parentObjectId, string? query)
        {
            throw new NotImplementedException();
        }
    }
}

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

                result = houses.Select(HouseToSearchModel).ToList();
            }
            else
            {
                result = addresses.Select(AddressToSearchModel).ToList();
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

        public async Task<List<SearchAddressModel>> GetAddressChain(string? objectId)
        {
            var result = new List<SearchAddressModel>();

            if (objectId == null)
            {
                throw new NotFoundException(ErrorMessages.AddressNotFound);
            }

            var addressObject = await _db.AsAddrObjs
                    .FirstOrDefaultAsync(a => a.ObjectGuid.ToString() == objectId);

            long currentObjectId;
            if (addressObject == null)
            {
                var housesObject = await _db.AsHouses
                    .FirstOrDefaultAsync(h => h.ObjectGuid.ToString() == objectId);

                if (housesObject == null)
                {
                    throw new NotFoundException(ErrorMessages.AddressNotFound);
                }

                currentObjectId = housesObject.Parentobjid;
                result.Add(HouseToSearchModel(housesObject));
            }
            else
            {
                currentObjectId = addressObject.Parentobjid;
                result.Add(AddressToSearchModel(addressObject));
            }

            var currentObject = await _db.AsAddrObjs
                .FirstAsync(a => a.ObjectId == currentObjectId);
            while (currentObject != null)
            {
                result.Add(AddressToSearchModel(currentObject));
                currentObject = await _db.AsAddrObjs
                    .FirstOrDefaultAsync(a => a.ObjectId == currentObject.Parentobjid);
            }

            result.Reverse();
            return result;
        }

        private SearchAddressModel AddressToSearchModel(AsAddrObj asAddrObj)
        {
            return new SearchAddressModel
            {
                ObjectId = asAddrObj.ObjectId,
                ObjectGuid = asAddrObj.ObjectGuid,
                Text = AddressHelper.GetAddressName(asAddrObj.Typename, asAddrObj.Name),
                ObjectLevel = AddressHelper.GetAddressLevel(Convert.ToInt32(asAddrObj.Level)),
                ObjectLevelText = AddressHelper.GetAddressLevelName(Convert.ToInt32(asAddrObj.Level))
            };
        }

        private SearchAddressModel HouseToSearchModel(AsHouse asHouse)
        {
            return new SearchAddressModel
            {
                ObjectId = asHouse.ObjectId,
                ObjectGuid = asHouse.ObjectGuid,
                Text = AddressHelper.GetHouseName(asHouse),
                ObjectLevel = ObjectLevel.Building,
                ObjectLevelText = AddressHelper.GetAddressLevelName(10)
            };
        }
    }
}

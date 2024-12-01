using BackendLaboratory.Repository;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BackendLaboratory.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetAuthors(
            [FromQuery] long? parentObjectId,
            [FromQuery] string? query
            )
        {
            var response = await _addressRepository.SearchAdress(parentObjectId, query);
            return Ok(response);
        }

        [HttpGet("chain")]
        public async Task<IActionResult> GetAddressChain([FromQuery] string? objectId)
        {
            var response = await _addressRepository.GetAddressChain(objectId);
            return Ok(response);
        }
    }
}

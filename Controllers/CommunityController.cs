using BackendLaboratory.Repository;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BackendLaboratory.Controllers
{
    [Route("api/community")]
    [ApiController]
    public class CommunityController : Controller
    {
        private readonly ICommunityRepository _communityRepository;

        public CommunityController(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
        }

        [HttpGet(AppConstants.EmptyString)]
        public async Task<IActionResult> GetCommunities() 
        {
            var response = await _communityRepository.GetCommunities();
            return Ok(response);
        }
    }
}

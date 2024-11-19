using BackendLaboratory.Repository;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

        private string GetTokenFromHeader()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        [HttpGet(AppConstants.EmptyString)]
        public async Task<IActionResult> GetCommunities() 
        {
            var response = await _communityRepository.GetCommunities();
            return Ok(response);
        }

        [HttpPost("{id}/substribe")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> SubstribeToCommunity(string id)
        {
            string token = GetTokenFromHeader();

            await _communityRepository.SubstribeToCommunity(token, id);
            return Ok();
        }
    }
}

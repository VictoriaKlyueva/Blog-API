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

        [HttpGet("my")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> GetUserCommunities()
        {
            string token = GetTokenFromHeader();

            var response = await _communityRepository.GetUserCommunities(token);
            return Ok(response);
        }

        [HttpPost("{id}/subscribe")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> SubscribeToCommunity(string id)
        {
            string token = GetTokenFromHeader();

            await _communityRepository.SubscribeToCommunity(token, id);
            return Ok();
        }

        [HttpDelete("{id}/unsubscribe")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> UnubscribeFromCommunity(string id)
        {
            string token = GetTokenFromHeader();

            await _communityRepository.UnsubscribeFromCommunity(token, id);
            return Ok();
        }
    }
}

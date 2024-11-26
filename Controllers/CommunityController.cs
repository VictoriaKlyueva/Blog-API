using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities.Enums;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunityInfo(string id)
        {
            var response = await _communityRepository.GetCommunityInfo(id);
            return Ok(response);
        }

        [HttpGet("{id}/post")]
        public async Task<IActionResult> GetCommunityPosts(
            string id,
            [FromQuery] List<Guid>? tags,
            [FromQuery] PostSorting? sorting,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            string? token = GetTokenFromHeader();

            var response = await _communityRepository.GetCommunityPosts(tags, sorting, page, 
                size, token, id);
            return Ok(response);
        }

        [HttpGet("{id}/role")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> GetCommunityRole(string id)
        {
            string token = GetTokenFromHeader();

            var response = await _communityRepository.GetCommunityRole(token, id);
            return Ok(response);
        }

        [HttpPost("{id}/post")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> CreateCommunityPost([FromBody] CreatePostDto createPostDto, string id)
        {
            string token = GetTokenFromHeader();

            await _communityRepository.CreateCommunityPost(token, id, createPostDto);
            return Ok();
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

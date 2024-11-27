using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;
using BackendLaboratory.Repository;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLaboratory.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        private string? GetTokenFromHeader()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        [HttpGet(AppConstants.EmptyString)]
        public async Task<IActionResult> GetPosts(
            [FromQuery] List<Guid>? tags,
            [FromQuery] string? author,
            [FromQuery] int? min,
            [FromQuery] int? max,
            [FromQuery] PostSorting? sorting,
            [FromQuery] bool onlyMyCommunities = false,
            [FromQuery] int page = 1,
            [FromQuery] int size = 5
            )
        {
            string? token = GetTokenFromHeader();

            var response = await _postRepository.GetPosts(tags,author, min, max,
                sorting, onlyMyCommunities, page, size, token);
            return Ok(response);
        }

        [HttpPost(AppConstants.EmptyString)]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto model)
        {
            string token = GetTokenFromHeader();

            await _postRepository.CreatePost(token, model);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostInfo(string id)
        {
            string? token = GetTokenFromHeader();

            var responce = await _postRepository.GetPostInfo(token, id);
            return Ok(responce);
        }

        [HttpPost("{postId}/like")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> AddLike(string postId)
        {
            string token = GetTokenFromHeader();

            await _postRepository.AddLike(token, postId);
            return Ok();
        }

        [HttpDelete("{postId}/like")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> DeleteLike(string postId)
        {
            string token = GetTokenFromHeader();

            await _postRepository.DeleteLike(token, postId);
            return Ok();
        }
    }
}

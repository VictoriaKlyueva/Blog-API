using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
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

        private string GetTokenFromHeader()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        [HttpPost(AppConstants.EmptyString)]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto model)
        {
            string token = GetTokenFromHeader();

            await _postRepository.CreatePost(token, model);
            return Ok();
        }
    }
}

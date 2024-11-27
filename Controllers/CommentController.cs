using BackendLaboratory.Data.DTO;
using BackendLaboratory.Repository;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLaboratory.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        private string GetTokenFromHeader()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        [HttpPost("post/{id}/comment")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto createCommentDto, 
            string id)
        {
            string token = GetTokenFromHeader();

            await _commentRepository.AddComment(id, token, createCommentDto);
            return Ok();
        }

        [HttpPut("comment/{id}")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> EditComment([FromBody] UpdateCommentDto updateCommentDto,
            string id)
        {
            string token = GetTokenFromHeader();

            await _commentRepository.EditComment(id, token, updateCommentDto);
            return Ok();
        }
    }
}

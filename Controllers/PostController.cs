﻿using BackendLaboratory.Data.DTO;
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

        [HttpGet("{id}")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> GetPostInfo(string id)
        {
            string token = GetTokenFromHeader();

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

using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BackendLaboratory.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials model)
        {
            var tokenResponse = await _userRepository.Login(model);
            if (string.IsNullOrEmpty(tokenResponse.Token)) 
            {
                return BadRequest();
            }

            return Ok(tokenResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            if (!_userRepository.IsUniqueUser(model.Email))
            {
                return BadRequest();
            }

            var token = await _userRepository.Register(model);

            if (token == null)
            {
                return BadRequest();
            }

            return Ok(token);
        }

        [HttpPost("logout")]
        [Authorize(Policy = "TokenBlackListPolicy")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required for logout.");
            }

            await _userRepository.Logout(token);
            return Ok();
        }
    }
}

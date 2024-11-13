using BackendLaboratory.Data.DTO;
using BackendLaboratory.Models;
using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;

namespace BackendLaboratory.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response; 

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this._response = new();
        }

        private void FillResponse(HttpStatusCode StatusCode, bool IsSucesses, 
            string? ErrorMessage=null, object? Result=null)
        {
            _response = new APIResponse();
            _response.StatusCode = StatusCode;
            _response.IsSucesses = IsSucesses;
            if (ErrorMessage != null)
                _response.ErrorMessages.Add(ErrorMessage);
            if (Result != null)
                _response.Result = Result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token)) 
            {
                FillResponse(HttpStatusCode.BadRequest, false, "Username or password is incorrect", null);
                return BadRequest(_response);
            }
            FillResponse(HttpStatusCode.OK, true, null, loginResponse);
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            if (!_userRepository.IsUniqueUser(model.UserName))
            {
                FillResponse(HttpStatusCode.BadRequest, false, "Username already exists", null);
                return BadRequest(_response);
            }

            var user = await _userRepository.Register(model);

            if (user == null)
            {
                FillResponse(HttpStatusCode.BadRequest, false, "Error while registering", null);
                return BadRequest(_response);
            }

            FillResponse(HttpStatusCode.OK, true, null, user);
            return Ok(_response);
        }
    }
}

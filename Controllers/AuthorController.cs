using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BackendLaboratory.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAuthors()
        {
            var response = await _authorRepository.GetAuthors();
            return Ok(response);
        }
    }
}

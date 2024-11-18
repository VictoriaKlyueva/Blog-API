using BackendLaboratory.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BackendLaboratory.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet("tag")]
        public async Task<IActionResult> GetTags()
        {
            var response = await _tagRepository.GetTags();
            return Ok(response);
        }
    }
}

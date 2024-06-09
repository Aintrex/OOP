using Lib.ModelReq;
using Lib.Models;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("getAllAuthors")]
        public async Task<IActionResult> GetAuthors()
        {
            IEnumerable<string> auts = await _authorService.GetAllAuthors();
            return Ok(auts);
        }
        [HttpPost("addAuthor")]
        public async Task<IActionResult> Createaut([FromBody] AuthorCreate model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if (!_authorService.ValidString(model.Name)) { return BadRequest(new { success = false, message = "Author field can't contain digits or special symbols!" }); }
            }
            int aut = await _authorService.CreateAuthor(model.Name);

            if (aut != 0)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false, message = "Author could not be added. It might already exist." });
            }
        // GET: api/<AuthorsController>
    }
}

using Lib.ModelReq;
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
        [HttpPost("addAuthor")]
        public async Task<IActionResult> Createaut([FromBody] AuthorCreate model)
        {
            int aut = await _authorService.CreateAuthor(model.Name);

            if (aut != 0)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false });
        }
        // GET: api/<AuthorsController>
    }
}

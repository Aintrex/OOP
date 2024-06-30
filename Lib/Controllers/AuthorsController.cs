using Lib.ModelReq;
using Lib.Models;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
        [HttpDelete("deleteAuthor")]
        public IActionResult DeleteAut(string aut)
        {
            Author author = _authorService.GetAuthorByName(aut);
            if (author == null)
            {
                return NotFound(new { success = false, message = "Author not found" });
            }

            // Check if the author is associated with any books
            var booksWithAuthor = _authorService.AssociatedAut(author.Id);
            if (booksWithAuthor)
            {
                return BadRequest(new { success = false, message = "Cannot delete author because they are associated with books" });
            }

            if (!_authorService.Deleteauthor(aut))
                return NotFound();
            return Ok(new { success = true, message = "Author deleted successfully" });
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

using Lib.ModelReq;
using Lib.Models;
using Lib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

       // GET api/<BooksController>/5
        [HttpGet("search")]
        public async Task<IActionResult> GetBooksFilter(string? title, string? autName, string? publisher, string? country, string? genre, string? lang, int? year)
        {
            IEnumerable<Book> books = await _bookService.GetBooksFilterAsync(title,autName,publisher,country,genre,lang,year);
            return Ok(books);
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreate model)
        {
            int book = await _bookService.CreateBook(model);

            if (book != 0)
            {
                return Ok("The book was successfully added to the database");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "The book was successfully added to the database");
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

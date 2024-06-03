using Lib.ModelReq;
using Lib.Models;
using Lib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            IEnumerable<Book> books = await _bookService.GetBooksFilterAsync(title, autName, publisher, country, genre, lang, year);
            return Ok(books);
        }

        [HttpPost("updateBook")]
        public IActionResult UpdateBook([FromBody] BookUpdate request)
        {
            _bookService.UpdateBook(request.Id, request.Title, request.AuthorId, request.YearId, request.CountryId, request.PublisherId, request.LanguageId, request.GenreId);
            return Ok(new { success = true });
        }
        // POST api/<BooksController>
        [HttpPost("addBook")]
        public async Task<IActionResult> CreateBook([FromBody] BookCreate request)
        {
            int book = await _bookService.CreateBook(request.Title, request.AuthorId, request.YearId, request.CountryId, request.PublisherId, request.LanguageId, request.GenreId);

            if (book != 0)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false });
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

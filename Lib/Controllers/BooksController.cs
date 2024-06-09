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
        [HttpDelete]
        public async Task<IActionResult> DeleteBook(string title)
        {
            var book = _bookService.DeleteBook(title);
            if(book)
            {
                return Ok(new { success = true, message = "Book deleted successfully" });
            }
            return BadRequest(new {success=false, message = "Book doesn't exist"});
        }
        [HttpGet("getAllTitles")]
        public IActionResult GetAllBooks()
        {
            IEnumerable<string> books = _bookService.GetAllBooksTitles();
            return Ok(books);
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
            //if (!string.IsNullOrWhiteSpace(request.NewCountry) || !string.IsNullOrWhiteSpace(request.NewLanguage) || !string.IsNullOrEmpty(request.NewAuthor) || !string.IsNullOrEmpty(request.NewGenre))
            //{
            //    if (!_bookService.ValidString(request.NewCountry) && !_bookService.ValidString(request.NewLanguage) && !_bookService.ValidString(request.NewAuthor) && !_bookService.ValidString(request.NewGenre))
            //    {
            //        return BadRequest(new { success = false, message = "Author, Language, Country and Genre fields cannot contain digits or special symbols." });
            //    }
            //}

            if (string.IsNullOrWhiteSpace(request.NewTitle) && string.IsNullOrWhiteSpace(request.NewAuthor) &&
                request.NewYear == null && string.IsNullOrWhiteSpace(request.NewCountry) &&
                string.IsNullOrWhiteSpace(request.NewPublisher) && string.IsNullOrWhiteSpace(request.NewLanguage) &&
                string.IsNullOrWhiteSpace(request.NewGenre))
            {
                return BadRequest(new { success = false, message = "At least one new field must be filled." });
            }

            var result = _bookService.UpdateBook(request.ExistingTitle, request.NewTitle, request.NewAuthor, request.NewYear, request.NewCountry, request.NewPublisher, request.NewLanguage, request.NewGenre);
            if (result)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Book or one of Parametrs not found." });
            }
        }
        // POST api/<BooksController>
        [HttpPost("addBook")]
        public async Task<IActionResult> CreateBook([FromBody] BookCreate request)
        {
            int book = await _bookService.CreateBook(request.Title, request.Author, request.Year, request.Country, request.Publisher, request.Language, request.Genre);

            if (book != 0)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false, message = "Failed to add book. It's might already exist" });
        }
    }
}

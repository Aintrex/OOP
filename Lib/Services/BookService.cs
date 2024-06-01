using Lib.ModelReq;
using Lib.Models;
using Lib.Context;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();

        Task<Book> GetBookByIdAsync(int id);

        Task<int> CreateBook(BookCreate model);

        Task DeleteBook(int id);
        Task<List<Book>> GetBooksFilterAsync(string? title, string? autName, string? publisher, string? country, string? genre, string? lang, int? year);
    }
    public class BookService : IBookService
    {
        private LibContext _dbContext;


        public BookService(LibContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Book>> GetBooksFilterAsync(string? title, string? autName, string? publisher, string? country, string? genre, string? lang, int? year)
        {
            var booksQuery = _dbContext.Books.Include(b => b.author).Include(b => b.publisher).Include(b => b.country).Include(b => b.Genre).Include(b => b.language).Include(b => b.ReleaseYear) as IQueryable<Book>;

            if (!string.IsNullOrEmpty(title))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(autName))
            {
                booksQuery = booksQuery.Where(b => b.author.Name.Contains(autName));
            }

            if (!string.IsNullOrEmpty(publisher))
            {
                booksQuery = booksQuery.Where(b => b.publisher.Name.Contains(publisher));
            }

            if (!string.IsNullOrEmpty(country))
            {
                booksQuery = booksQuery.Where(b => b.country.Name.Contains(country));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                booksQuery = booksQuery.Where(b => b.Genre.Name.Contains(genre));
            }

            if (!string.IsNullOrEmpty(lang))
            {
                booksQuery = booksQuery.Where(b => b.language.Name.Contains(lang));
            }

            if (year.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.ReleaseYear.Year == year.Value);
            }
            return await booksQuery.ToListAsync();

        }
        public async Task<int> CreateBook(BookCreate model)
        {

            
            Book book = new Book
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                CountryId = model.CountryId,
                GenreId = model.GenreId,
                LanguageId = model.LanguageId,
                PublisherId = model.PublisherId,
                YearId = model.YearId
            };


            
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync().ConfigureAwait(true);

            return book.Id;
        }
            


    }
}

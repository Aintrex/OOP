using Lib.ModelReq;
using Lib.Models;
using Lib.Context;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Lib.Services
{
    public interface IBookService
    {
      
        Task<int> CreateBook(string title, int authorId, int yearId, int countryId, int publisherId, int languageId, int genreId);
        Task<Book> GetBookById(int bookId);
        Task<List<Book>> GetBooksFilterAsync(string? title, string? autName, string? publisher, string? country, string? genre, string? lang, int? year);
        void UpdateBook(int bookId, string title, int authorId, int yearId, int countryId, int publisherId, int languageId, int genreId);
        Task DeleteBook(int bookId);
    }
    public class BookService : IBookService
    {
        private LibContext _dbContext;


        public BookService(LibContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteBook(int bookId)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x=>x.Id == bookId);
            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
        }
        public async Task<Book> GetBookById(int bookId)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book was not found in database");
            }

            return book;
        }
        public void UpdateBook(int bookId, string title, int authorId, int yearId, int countryId, int publisherId, int languageId, int genreId)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                book.Title = title;
                book.AuthorId = authorId;
                book.YearId = yearId;
                book.CountryId = countryId;
                book.PublisherId = publisherId;
                book.LanguageId = languageId;
                book.GenreId = genreId;
                _dbContext.Books.Update(book);
                _dbContext.SaveChanges();
            }
          
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
        public async Task<int> CreateBook(string title, int authorId, int yearId, int countryId, int publisherId, int languageId, int genreId)
        {
            if (await _dbContext.Books.AnyAsync(x => x.Title == title))
                return 0;
            
            Book book = new Book
            {
                Title = title,
                AuthorId = authorId,
                CountryId = countryId,
                GenreId = genreId,
                LanguageId = languageId,
                PublisherId = publisherId,
                YearId = yearId
            };


            
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return book.Id;
        }
            


    }
}

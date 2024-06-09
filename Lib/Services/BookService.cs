using Lib.ModelReq;
using Lib.Models;
using Lib.Context;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Lib.Services
{
    public interface IBookService
    {
      
        Task<int> CreateBook(string title, string author, int? year, string country, string publisher, string language, string genre);
        Task<Book> GetBookById(int bookId);
        Task<List<Book>> GetBooksFilterAsync(string? title, string? autName, string? publisher, string? country, string? genre, string? lang, int? year);
        bool UpdateBook(string existTitle, string title, string  author, int? year, string country, string publisher, string language, string genre);
        List<string> GetAllBooksTitles();
        bool ValidString(string str);
        bool DeleteBook(string title);
    }
    public class BookService : IBookService
    {
        private LibContext _dbContext;


        public BookService(LibContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool DeleteBook(string title)
        {
            var book = _dbContext.Books.FirstOrDefault(x=>x.Title == title);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
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
        public List<string> GetAllBooksTitles()
        {
            //return _dbContext.Books.ToList();
            var books = _dbContext.Books.OrderBy(x => x.Title).ToList();
            return /*_dbContext.Books.OrderBy(x => x.Title).Select(b => b.Title).Distinct().ToList();*/ books.Select(x => x.Title).ToList();
        }
        public  bool ValidString(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[a-zA-Zа-яА-ЯёЁ\\s]+$");
            return regex.IsMatch(str);
        }
        public bool UpdateBook(string existTitle, string newtitle, string  sauthor, int? iyear, string scountry, string spublisher, string slanguage, string sgenre)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Title==existTitle);
         
            if (book != null)
            {
                if(!string.IsNullOrWhiteSpace(sauthor) && !_dbContext.Authors.Any(a => a.Name == sauthor))
                    return false;

                if (iyear.HasValue && !_dbContext.Releaseyears.Any(y => y.Year == iyear.Value))
                    return false;

                if (!string.IsNullOrWhiteSpace(scountry) && !_dbContext.Countries.Any(c => c.Name == scountry))
                    return false;

                if (!string.IsNullOrWhiteSpace(spublisher) && !_dbContext.Publishers.Any(p => p.Name == spublisher))
                    return false;

                if (!string.IsNullOrWhiteSpace(slanguage) && !_dbContext.Languages.Any(l => l.Name == slanguage))
                    return false;

                if (!string.IsNullOrWhiteSpace(sgenre) && !_dbContext.Genres.Any(g => g.Name == sgenre))
                    return false;
                if (!string.IsNullOrWhiteSpace(newtitle)) book.Title = newtitle;
                if (!string.IsNullOrWhiteSpace(sauthor))
                {
                    var aut = _dbContext.Authors.FirstOrDefault(a => a.Name == sauthor);
                    book.AuthorId = aut.Id;
                }
                if (iyear.HasValue)/* book.ReleaseYear.Year = iyear.Value*/
                {
                    var yr = _dbContext.Releaseyears.FirstOrDefault(ry=>ry.Year==iyear.Value);
                    book.YearId = yr.Id;
                }
                if (!string.IsNullOrWhiteSpace(scountry))
                {
                    var cou = _dbContext.Countries.FirstOrDefault(c => c.Name == scountry);
                    book.CountryId = cou.Id;
                }
                if (!string.IsNullOrWhiteSpace(spublisher))
                {
                    var pbl = _dbContext.Publishers.FirstOrDefault(p => p.Name == spublisher);
                    book.PublisherId = pbl.Id;
                }
                if (!string.IsNullOrWhiteSpace(slanguage))
                { 
                    var lng = _dbContext.Languages.FirstOrDefault(l=>l.Name==slanguage);
                    book.LanguageId = lng.Id;
                }
                if (!string.IsNullOrWhiteSpace(sgenre))
                {
                    var gnr = _dbContext.Genres.FirstOrDefault(g=>g.Name==sgenre);
                    book.GenreId = gnr.Id;
                }
                _dbContext.Books.Update(book);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
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
        public async Task<int> CreateBook(string title, string author, int? year, string country, string publisher, string language, string genre)
        {

            if (await _dbContext.Books.AnyAsync(x => x.Title == title))
            {
                var chb = await _dbContext.Books.Include(x=>x.author).FirstOrDefaultAsync(x => x.Title == title);
                if (chb.author.Name==author)
                return 0;
            }
            var aut = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Name == author);
            var cnt = await _dbContext.Countries.FirstOrDefaultAsync(x => x.Name == country);
            var gnr = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Name == genre);
            var lng = await _dbContext.Languages.FirstOrDefaultAsync(x => x.Name == language);
            var pbl = await _dbContext.Publishers.FirstOrDefaultAsync(x => x.Name == publisher);
            var yr = _dbContext.Releaseyears.FirstOrDefault(ry => ry.Year == year.Value);
            Book book = new Book
            {
                Title = title,
                AuthorId = aut.Id,
                CountryId = cnt.Id,
                GenreId = gnr.Id,
                LanguageId = lng.Id,
                PublisherId = pbl.Id,
                YearId = yr.Id
            };


            
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return book.Id;
        }
            


    }
}

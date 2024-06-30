using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IGenreService
    {
        Task<int> CreateGenre(string name);
        Task<List<string>> GetAllGenres();
        bool ValidString(string str);
        Genre GetGenreByName(string genre);
        bool AssociatedGen(int gid);
        bool DeleteGenre(string name);
    }
    public class GenreService : IGenreService
    {
        private readonly LibContext _dbContext;
        public GenreService(LibContext context)
        {
            _dbContext = context;
        }
        public bool DeleteGenre(string name)
        {
            var genre = _dbContext.Genres.FirstOrDefault(x => x.Name == name);
            if (genre == null)
            {
                return false;
            }
            _dbContext.Genres.Remove(genre);
            _dbContext.SaveChanges();
            return true;
        }
        public bool AssociatedGen(int gid)
        {
            if (_dbContext.Books.Any(x => x.GenreId == gid))
                return true;
            return false;
        }
        public Genre GetGenreByName(string name)
        {
            var genr = _dbContext.Genres.FirstOrDefault(x=>x.Name== name);
            return genr;
        }
        public bool ValidString(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[a-zA-Zа-яА-Я\\s]+$");
            return regex.IsMatch(str);
        }
        public async Task<List<string>> GetAllGenres()
        {
            var gnr = await _dbContext.Genres.OrderBy(x => x.Name).ToListAsync();
            return gnr.Select(x=>x.Name).ToList();
        }
        public async Task<int> CreateGenre(string name)
        {
            name = string.Join(" ", name.Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
            if (await _dbContext.Genres.AnyAsync(x => x.Name == name)) { return 0; }
            Genre gen = new Genre
            {
                Name = name
            };
            _dbContext.Genres.Add(gen);
            _dbContext.SaveChanges();
            return gen.Id;
        }
    }
}

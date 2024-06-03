using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IGenreService
    {
        Task<int> CreateGenre(string name);
    }
    public class GenreService : IGenreService
    {
        private readonly LibContext _dbContext;
        public GenreService(LibContext context)
        {
            _dbContext = context;
        }
        public async Task<int> CreateGenre(string name)
        {
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

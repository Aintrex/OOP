using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IAuthorService
    {
        Task Deleteauthor(int authId);
        Task<int> CreateAuthor(string name);
        Task<List<string>> GetAllAuthors();
        bool ValidString(string check);
    }

    public class AuthorService : IAuthorService
    {
        private readonly LibContext _dbContext;
        public AuthorService(LibContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<string>> GetAllAuthors()
        {
            var aut = await _dbContext.Authors.OrderBy(x=>x.Name).ToListAsync();
            return aut.Select(x => x.Name).Distinct().ToList();
        }
        public bool ValidString(string check)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[a-zA-Zа-яА-ЯёЁ\\s]+$");
            return regex.IsMatch(check);
        }
        public async Task Deleteauthor(int authId)
        {
            var aut = await _dbContext.Authors.FirstOrDefaultAsync(x=>x.Id== authId);
            _dbContext.Authors.Remove(aut);
            _dbContext.SaveChanges();
        }
        public async Task<int> CreateAuthor (string name)
        {
            name = string.Join(" ", name.Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
            if (await _dbContext.Authors.AnyAsync(x => x.Name == name))
                return 0;
            Author aut = new Author
            {
                Name = name
            };



            _dbContext.Authors.Add(aut);
            await _dbContext.SaveChangesAsync();

            return aut.Id;
        }
    }
}

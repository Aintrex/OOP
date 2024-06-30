using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IAuthorService
    {
        bool Deleteauthor(string authId);
        Task<int> CreateAuthor(string name);
        Task<List<string>> GetAllAuthors();
        bool ValidString(string check);
        Author GetAuthorByName(string name);
        bool AssociatedAut(int aid);
    }

    public class AuthorService : IAuthorService
    {
        private readonly LibContext _dbContext;
        public AuthorService(LibContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool AssociatedAut(int aid)
        {
            if(_dbContext.Books.Any(x=>x.AuthorId==aid))
            {
                return true;
            }
            return false;
        }
        public Author GetAuthorByName(string name)
        {
            var autr =  _dbContext.Authors.FirstOrDefault(x => x.Name == name);
            return autr;
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
        public bool Deleteauthor(string authId)
        {
            var aut = _dbContext.Authors.FirstOrDefault(x=>x.Name== authId);
            if (aut == null)
                return false;
            _dbContext.Authors.Remove(aut);
            _dbContext.SaveChanges();
            return true;
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

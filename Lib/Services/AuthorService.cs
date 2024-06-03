using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IAuthorService
    {
        Task Deleteauthor(int authId);
        Task<int> CreateAuthor(string name);
    }

    public class AuthorService : IAuthorService
    {
        private readonly LibContext _dbContext;
        public AuthorService(LibContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Deleteauthor(int authId)
        {
            var aut = await _dbContext.Authors.FirstOrDefaultAsync(x=>x.Id== authId);
            _dbContext.Authors.Remove(aut);
            _dbContext.SaveChanges();
        }
        public async Task<int> CreateAuthor (string name)
        {
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

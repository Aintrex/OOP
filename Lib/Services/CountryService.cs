using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface ICountryInterface
    {
        Task<int> CreateCountry(string name);
    }
    public class CountryService:ICountryInterface
    {
        private readonly LibContext _context;
        public CountryService(LibContext context)
        {
            _context = context;
        }
        public async Task<int> CreateCountry (string name)
        {
            if (await _context.Countries.AnyAsync(x=>x.Name== name)) { return 0; }
            Country cou = new Country
            {
                Name = name
            };
            _context.Countries.Add(cou);
            _context.SaveChanges();
            return cou.Id;
        }
    }
}

using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface ICountryInterface
    {
        Task<int> CreateCountry(string name);
        Task<List<string>> GetAllCountries();
        bool ValidString(string str);
        Country GetCountryByName(string name);
        bool AssociatedCnt(int cid);
        bool DeleteCountry(string cn);
    }
    public class CountryService:ICountryInterface
    {
        private readonly LibContext _context;
        public CountryService(LibContext context)
        {
            _context = context;
        }
        public bool DeleteCountry(string cn)
        {
            var cnt = _context.Countries.FirstOrDefault(x=>x.Name == cn);
            if (cnt == null)
                return false;
            _context.Countries.Remove(cnt);
            _context.SaveChanges();
            return true;
        }
        public bool AssociatedCnt(int cid)
        {
            if(_context.Books.Any(x=>x.CountryId == cid))
            {
                return true;
            }
            return false;
        }
        public Country GetCountryByName(string naem)
        {
            var count = _context.Countries.FirstOrDefault(x => x.Name == naem);
            return count;
        }
        public bool ValidString(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[a-zA-Zа-яА-ЯёЁ\\s]+$");
            return regex.IsMatch(str);
        }
        public async Task<List<string>> GetAllCountries()
        {
            var cont = await _context.Countries.OrderBy(x=>x.Name).ToListAsync();
            return cont.Select(x=>x.Name).ToList();
        }
        public async Task<int> CreateCountry (string name)
        {
            name = string.Join(" ", name.Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
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

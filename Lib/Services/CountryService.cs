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
    }
    public class CountryService:ICountryInterface
    {
        private readonly LibContext _context;
        public CountryService(LibContext context)
        {
            _context = context;
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

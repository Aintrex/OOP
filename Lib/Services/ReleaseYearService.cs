using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IReleaseYearInterface
    {
        Task<int> CreateRY(int year);
        Task<List<int>> GetAllYears();
    }
    public class ReleaseYearService:IReleaseYearInterface
    {
        private readonly LibContext _context;
        public ReleaseYearService(LibContext context) {  _context = context; }
        public async Task<List<int>> GetAllYears()
        {
            var yr = await _context.Releaseyears.OrderByDescending(x => x.Year).ToListAsync();
            return yr.Select(x=>x.Year).Distinct().ToList(); 
        }
        public async Task<int> CreateRY(int year)
        {
            if (await _context.Releaseyears.AnyAsync(x => x.Year == year)) { return 0; }
            ReleaseYear Ryear = new ReleaseYear
            {
                Year = year
            };
            _context.Releaseyears.Add(Ryear);
            _context.SaveChanges();
            return Ryear.Id;
        }
    }
}

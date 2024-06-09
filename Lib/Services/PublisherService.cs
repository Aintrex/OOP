using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IPublisherInterface
    {
        Task<int> CreatePub(string name);
        Task<List<string>> GetAllPublishers();
        bool ValidString(string str);
    }
    public class PublisherService:IPublisherInterface
    {
        private readonly LibContext _context;
        public PublisherService(LibContext context) {  _context = context; }
        public bool ValidString(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[a-zA-Zа-яА-ЯёЁ\\s]+$");
            return regex.IsMatch(str);
        }
        public async Task<List<string>> GetAllPublishers()
        {
            var pbl = await _context.Publishers.OrderBy(x => x.Name).ToListAsync();
            return pbl.Select(x=>x.Name).Distinct().ToList();
        }
        public async Task<int> CreatePub(string name)
        {
            name = string.Join(" ", name.Split(' ')
                .Where(x=>!string.IsNullOrEmpty(x))
                .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
            if (await _context.Publishers.AnyAsync(x => x.Name == name)) return 0;
            Publisher pub = new Publisher
            {
                Name = name
            };
            _context.Publishers.Add(pub);
            _context.SaveChanges();
            return pub.Id;
        }
    }
}

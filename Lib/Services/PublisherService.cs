using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IPublisherInterface
    {
        Task<int> CreatePub(string name);
    }
    public class PublisherService:IPublisherInterface
    {
        private readonly LibContext _context;
        public PublisherService(LibContext context) {  _context = context; }
        public async Task<int> CreatePub(string name)
        {
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

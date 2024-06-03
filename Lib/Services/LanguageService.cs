using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface ILanguageService
    {
        Task<int> CreateLang(string name);
    }
    public class LanguageService:ILanguageService
    {
        private readonly LibContext _context;
        public LanguageService(LibContext context)
        {
            _context = context;
        }
        public async Task<int> CreateLang(string name)
        {
            if(await _context.Languages.AnyAsync(x=>x.Name == name)) { return 0; }
            Language lang = new Language
            {
                Name = name
            };
            _context.Languages.Add(lang);
            _context.SaveChanges();
            return lang.Id;
        }
    }
}

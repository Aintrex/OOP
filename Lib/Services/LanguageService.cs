using Lib.Context;
using Lib.ModelReq;
using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface ILanguageService
    {
        Task<int> CreateLang(string name);
        Task<List<string>> GetAllLanguages();
        bool ValidString(string str);
        bool AssociatedLng(int aid);
        Language GetLanguageByName(string name);
        bool Deletelanguage(string lang);
    }
    public class LanguageService:ILanguageService
    {
        private readonly LibContext _context;
        public LanguageService(LibContext context)
        {
            _context = context;
        }
        public bool AssociatedLng(int aid)
        {
            if (_context.Books.Any(x => x.LanguageId == aid))
            {
                return true;
            }
            return false;
        }
        public Language GetLanguageByName(string name)
        {
            var laung = _context.Languages.FirstOrDefault(x => x.Name == name);
            return laung;
        }
        public bool Deletelanguage(string lang)
        {
            var lng = _context.Languages.FirstOrDefault(x => x.Name == lang);
            if (lng == null)
                return false;
            _context.Languages.Remove(lng);
            _context.SaveChanges();
            return true;
        }
        public bool ValidString(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[a-zA-Zа-яА-ЯёЁ\\s]+$");
            return regex.IsMatch(str);
        }
        public async Task<List<string>> GetAllLanguages()
        {
            var lng = await _context.Languages.OrderBy(x => x.Name).ToListAsync();
            return lng.Select(x=>x.Name).ToList();
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

using Lib.ModelReq;
using Lib.Models;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private ILanguageService _languageService;
        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpDelete("deleteLanguage")]
        public IActionResult DeleteLng(string lng)
        {
            Language language = _languageService.GetLanguageByName(lng);
            if (language == null)
            {
                return NotFound(new { success = false, message = "Author not found" });
            }

            // Check if the author is associated with any books
            var booksWithAuthor = _languageService.AssociatedLng(language.Id);
            if (booksWithAuthor)
            {
                return BadRequest(new { success = false, message = "Cannot delete author because they are associated with books" });
            }

            if (!_languageService.Deletelanguage(lng))
                return NotFound();
            return Ok(new { success = true, message = "Author deleted successfully" });
        }

        // GET api/<LanguagesController>/5
        [HttpGet("getAllLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            IEnumerable<string> lng = await _languageService.GetAllLanguages();
            return Ok(lng);
        }
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LanguagesController>
        [HttpPost("addLanguage")]
        public async Task<ActionResult> CreateLang([FromBody] LanguageCreate model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if (!_languageService.ValidString(model.Name)) { return BadRequest(new { success = false, message = "Language field can't contain digits or special symbols!" }); }
            }
            var lan = await _languageService.CreateLang(model.Name);
            if (lan !=0)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Language could not be added. It might already exist." });
        }

        // PUT api/<LanguagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LanguagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

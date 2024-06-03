using Lib.ModelReq;
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

        // GET: api/<LanguagesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LanguagesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LanguagesController>
        [HttpPost("addLanguage")]
        public async Task<ActionResult> CreateLang([FromBody] LanguageCreate model)
        {
            var lan = await _languageService.CreateLang(model.Name);
            if (lan !=0)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
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

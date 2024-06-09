using Lib.ModelReq;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private ICountryInterface _country;
        public CountriesController(ICountryInterface country)
        {
            _country = country;
        }

        // GET: api/<CountriesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CountriesController>/5
        [HttpGet("getAllCountries")]
        public async Task<IActionResult> GetCountries()
        {
            IEnumerable<string> cntr = await _country.GetAllCountries();
            return Ok(cntr);
        }

        // POST api/<CountriesController>
        [HttpPost("addCountry")]
        public async Task<IActionResult> CreateCountr([FromBody] CountryCreate model)
        {
            if(!string.IsNullOrEmpty(model.Name))
            {
                if(!_country.ValidString(model.Name)) { return BadRequest(new { sucess = false, message = "Country field cant' contain digits or special symbols" }); }
            }
            int contry = await _country.CreateCountry(model.Name);
            if (contry !=0)
            {
                return Ok(new {success=true});
            }
            return Ok(new { success = false, message = "Country could not be added. It might already exist. " });
        }

        // PUT api/<CountriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CountriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

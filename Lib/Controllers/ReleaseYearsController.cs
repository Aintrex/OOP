using Lib.ModelReq;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReleaseYearsController : ControllerBase
    {
        private IReleaseYearInterface _ryServ;
        public ReleaseYearsController(IReleaseYearInterface ryServ)
        {
            _ryServ = ryServ;
        }

        // GET: api/<ReleaseYearsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ReleaseYearsController>/5
        [HttpGet("getAllYears")]
       public async Task<IActionResult> GetYears()
        {
            IEnumerable<int> yrs = await _ryServ.GetAllYears();
            return Ok(yrs);
        }

        // POST api/<ReleaseYearsController>
        [HttpPost("addYear")]
       public async Task<IActionResult> CreateRyr([FromBody] ReleaseYearCreate model)
        {
            if (model.Year<1 || model.Year>DateTime.Now.Year)
                return Ok(new {success = false, message = "Year can't be lower 1 or higher current year"});
            var ry = await _ryServ.CreateRY(model.Year);
            if (ry != 0)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Year could not be added. It might already exist." });
        }

        // PUT api/<ReleaseYearsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReleaseYearsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

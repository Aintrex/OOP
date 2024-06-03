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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReleaseYearsController>
        [HttpPost("addYear")]
       public async Task<IActionResult> CreateRyr([FromBody] ReleaseYearCreate model)
        {
            var ry = await _ryServ.CreateRY(model.Year);
            if (ry != 0)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
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

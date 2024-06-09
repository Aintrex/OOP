using Lib.ModelReq;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private IPublisherInterface _pubService;
        public PublishersController(IPublisherInterface pubService)
        {
            _pubService = pubService;
        }
        // GET: api/<PublishersController>
        [HttpGet("getAllPublishers")]
        public async Task<IActionResult> GetPublishers()
        {
            IEnumerable<string> pbl = await _pubService.GetAllPublishers();
            return Ok(pbl);
            }
        // GET api/<PublishersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PublishersController>
        [HttpPost("addPublisher")]
       public async Task<IActionResult> CreatePubl([FromBody] PublisherCreate model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if(!_pubService.ValidString(model.Name)) { return BadRequest(new { success = false, message = "Publisher field can't contain digits or special symbols!" }); }
            }
            var pub = await _pubService.CreatePub(model.Name);
            if (pub != 0)
                return Ok(new {success = true});
            return Ok(new { success = false, message= "Publisher could not be added. It might already exist." });
        }

        // PUT api/<PublishersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PublishersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

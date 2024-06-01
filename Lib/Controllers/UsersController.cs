using Lib.Context;
using Lib.Models;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController (IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UserssController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserssController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserssController>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm]string username,[FromForm] string password)
        {
            var user = await _userService.CreateUserAsync(username, password);

            if (user !=0)
            {
                return Ok();
            }
            return BadRequest();
        }


        // PUT api/<UserssController>/5
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromForm] string username,[FromForm] string password)
        {
            var role = await _userService.LoginUserAsync(username, password);

            if (role != "Error")
            {
                return Ok(new { role });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // DELETE api/<UserssController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

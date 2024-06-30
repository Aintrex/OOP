using Lib.Context;
using Lib.Models;
using Lib.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
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
        public async Task<IActionResult> RegisterUser([FromForm] string username, [FromForm] string password)
        {
            var user = await _userService.CreateUserAsync(username, password);
            if (user == -1)
                return BadRequest(new { success = false, message = "Registartion is failed. Username can contain only letters, digits or _" });
            if (user != 0)
            {
                return Ok(new { success = true }); ;
            }
            return BadRequest(new {success = false, message = "Registartion is failed. User with this name already exists" });
        }


        // PUT api/<UserssController>/5
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromForm] string username, [FromForm] string password)
        {
            
            var role = await _userService.LoginUserAsync(username, password);
            if (role != "Error")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
                };
                var claimidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var autharisProp = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimidentity), autharisProp);
                return Ok(new { role, message = "Login successful" });
            }
                return Unauthorized(new { message = "Invalid username or password" });
        }
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); return Ok(new {message = "Logout successful"});
        }
        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                return Ok(new { isAuthenticated = true, role });
            }

            return Unauthorized(new { isAuthenticated = false });
        }
        [HttpGet("getLogin")]
        public async Task<IActionResult> GetLoginUsera()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.FindFirst(ClaimTypes.Name)?.Value;
                return Ok(new { name = name });
            }
            return BadRequest();
        }
        // DELETE api/<UserssController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Lib.Models;
using Lib.Context;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Lib.Services
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(string username, string password);

        Task<string> LoginUserAsync(string username, string password);
    }

    public class UserService : IUserService
    {
        private LibContext _libContext;
        public UserService(LibContext libContext)
        {
            _libContext = libContext;
        }
        public async Task<int> CreateUserAsync(string username, string password)
        {
            if( await _libContext.Users.AnyAsync(x => x.Login == username))
            {
                return 0;
            }


            User user = new User
            {
                Login = username,
                Password = password
            };
            if (user.Login == "Kuks")
            {
                user.Role = "admin";
            }
            else user.Role = "user";

            await _libContext.Users.AddAsync(user);

            await _libContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task<string> LoginUserAsync(string username, string password)
        {
            if (await _libContext.Users.AnyAsync(x=>x.Login==username))
            {
                var checkuser = await _libContext.Users.FirstOrDefaultAsync(x => x.Login == username);
                if (checkuser.Password ==password) 
                {
                    return checkuser.Role;
                }
            }
            return "Error";
        }

    }
}

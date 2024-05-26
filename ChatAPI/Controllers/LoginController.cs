using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ChatDbContext context;

        public LoginController(ChatDbContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(new {message = "Invalid data. Try again"});
            }

            User existUser = context.Users
                .SingleOrDefault(a => a.Email == user.Email || a.UserName == user.UserName);

            if(existUser != null)
            {
                return Conflict(new { message = "Username or Email already exist" });
            }


            

            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.Name,
                Password = user.Password,                
            };

            context.Users.Add(newUser);

            int result = context.SaveChanges();

            if (result == 0)
            {
                return StatusCode(500,new { message = "Server-side error.User creation failed" });
            }

            return Ok(new {message = "Complete successfully " });
        }
    }
}

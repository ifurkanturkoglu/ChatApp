using ChatAPI.Models;
using ChatAPI.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ChatDbContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        IOptions<TokenOption> tokenOption;
        public LoginController(ChatDbContext _context,UserManager<User> _userManager,SignInManager<User> _signInManager,IOptions<TokenOption> _tokenOption)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            tokenOption = _tokenOption;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new {message = "Invalid data. Try again"});
            }

            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.Name,
                Password = user.Password,
                UserCreatedTime = DateTime.Now,
            };

            var result = await userManager.CreateAsync(newUser, newUser.Password);

            if (!result.Succeeded) {
                return StatusCode(500, new { message = "Server-side error.User creation failed" });
            }
            

            return Ok(new {message = "Complete successfully " });
        }


        [HttpPost("api/auth/signin")]
        public async Task<IActionResult> SignIn(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = await userManager.FindByEmailAsync(loginModel.EmailOrUsername) 
                ?? await userManager.FindByNameAsync(loginModel.EmailOrUsername);
          

            if(user == null)
            {
                return BadRequest("User not found");
            }
            //IsPersistent beni hatırla seçeneği koy 
            var result = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password,lockoutOnFailure:false);

            if (result.Succeeded)
            {
                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer:tokenOption.Value.Issuer,
                    audience:tokenOption.Value.Audience,
                    claims:claims,
                    expires: DateTime.Now.AddDays(tokenOption.Value.Expiration),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(tokenOption.Value.SecretKey)),SecurityAlgorithms.HmacSha256 )             
                );

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                string userToken = tokenHandler.WriteToken(token);

                return Ok(userToken);
            }

            return BadRequest(new { message = "Error" });
        }
    }
}

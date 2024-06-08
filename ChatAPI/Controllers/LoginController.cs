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

        
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        IOptions<TokenOption> tokenOption;
        public LoginController( UserManager<User> _userManager, SignInManager<User> _signInManager, IOptions<TokenOption> _tokenOption)
        {
            
            userManager = _userManager;
            signInManager = _signInManager;
            tokenOption = _tokenOption;
        }

        [HttpPost("/api/login/signup")]
        public async Task<IActionResult> SignUp([FromBody]RegisterViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data. Try again" });
            }
            
            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.Username,
                PasswordHash = user.Password,
                UserCreatedTime = DateTime.Now,
            };

            var result = await userManager.CreateAsync(newUser, newUser.PasswordHash);

            if (!result.Succeeded) {
                return StatusCode(500, new { message = result.Errors.First() });
            }


            return Ok(new { message = "Complete successfully " });
        }


        [HttpPost("/api/login/signin")]
        public async Task<IActionResult> SignIn([FromBody]LoginViewModel loginModel)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = await userManager.FindByNameAsync(loginModel.EmailOrUsername)
                ?? await userManager.FindByEmailAsync(loginModel.EmailOrUsername);
                 


            if (user == null)
            {
                return BadRequest("User not found");
            }
            //IsPersistent beni hatırla seçeneği koy 
            var result = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: tokenOption.Value.Issuer,
                    audience: tokenOption.Value.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(tokenOption.Value.Expiration),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(tokenOption.Value.SecretKey)), SecurityAlgorithms.HmacSha256)
                );

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                string userToken = tokenHandler.WriteToken(token);

                return Ok(new { token = userToken });
            }

            return new JsonResult(new { loginModel.EmailOrUsername,loginModel.Password });
        }
    }
}

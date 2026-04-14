using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Framework;
using API_Arch_Framework.Areas.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPIArchitecture.Controllers.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            AppUser? userr = null;
            if (!string.IsNullOrEmpty(model.UserName) &&
                model.UserName.All(char.IsDigit) &&
                model.UserName.Length == 10)
            {
                userr = _userManager.Users.Where(x => x.PhoneNumber == model.UserName || x.UserName == model.UserName).FirstOrDefault();
            }
            else if(!string.IsNullOrEmpty(model.UserName))
            {
                userr = _userManager.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();
            }
            else
            {
                userr = await _userManager.FindByEmailAsync(model.UserName);
            }
            
            if (userr != null)
            {
                var result = await _signInManager.PasswordSignInAsync(userr.UserName, model.Password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userr.UserName);
                    var token = GenerateJwtToken(user);
                    var roles = await _userManager.GetRolesAsync(user);

                    return Ok(new
                    {
                        token,
                        user = new
                        {
                            user.Id,
                            user.UserName,
                            user.Email,
                            Roles = roles
                        }
                    });
                }

                var userCheck = await _userManager.FindByNameAsync(model.UserName);
                var failedAttempts = userCheck != null ? await _userManager.GetAccessFailedCountAsync(userCheck) : 0;

                if (result.IsLockedOut)
                    return Unauthorized(new { message = "Account locked. Try again later." });

                return Unauthorized(new
                {
                    message = "Invalid password",
                    accessFailedCount = failedAttempts
                });
            }
            else
            {
                return Unauthorized(new
                {
                    message = "Invalid email or phone number"
                });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "User signed out successfully" });
        }


        [Authorize]
        [HttpPost("addUser")]
        public async Task<IActionResult> Register([FromBody] AppUserDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = _roleManager.Roles.Where(x => x.Id == model.RoleId).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, role.Name != null ? role.Name : "User");

                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }
               
        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_DataAccessLayer;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_Framework.Areas.Identity;
using API_Arch_Framework.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIArchitecture.Controllers.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> usreManager, IUserServices userServices, IMapper mapper)
        {
            _userManager = usreManager;
            _userServices = userServices;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userServices.GetAllAsync();
            return Ok(result);
        }

        //[HttpPost("create")]
        //public async Task<IActionResult> CreateUser([FromBody] AppUserDto appUserDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(Result.Failure(new Error("Invalid user data")));

        //    try
        //    {
        //        var appUser = _mapper.Map<AppUser>(appUserDto);

        //        var result = await _userManager.CreateAsync(appUser, appUserDto.Password);

        //        if (result.Succeeded)
        //        {
        //            return Ok(Result.Success());
        //        }

        //        var errors = result.Errors.Select(e => e.Description).ToList();
        //        return BadRequest(Result.Failure(new Error(string.Join("; ", errors))));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, Result.Failure(new Error($"Internal error: {ex.Message}")));
        //    }
        //}

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);
            var result = _userServices.UpdateAsync(appUser);
            return Ok(result);
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(AppUserDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);
            var result = _userServices.UpdateAsync(appUser);
            return Ok(result);
        }
    }
}

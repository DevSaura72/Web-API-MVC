using API_Arch_Core.DataBaseObjects.Areas.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API_Arch_Framework.Areas.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebAPIArchitecture.Controllers.Areas.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleController(RoleManager<AppRole> roleManager, IMapper mapper)
        { _roleManager = roleManager; _mapper = mapper; }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _roleManager.Roles.ToListAsync());
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AppRoleDto appRoleDto)
        {
            var appRole = _mapper.Map<AppRole>(appRoleDto);
            return Ok(_roleManager.CreateAsync(appRole));
        }

        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole(AppRoleDto appRoleDto)
        {
            var appRole = _mapper.Map<AppRole>(appRoleDto);
            return Ok(_roleManager.UpdateAsync(appRole));
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole(AppRoleDto appRoleDto)
        {
            var appRole = _mapper.Map<AppRole>(appRoleDto);
            return Ok(_roleManager.UpdateAsync(appRole));
        }

        //private IActionResult Ok(object value)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById(int Id)
        {
            return Ok(_roleManager.Roles.Where(x => x.Id == Id));
        }

    }
}

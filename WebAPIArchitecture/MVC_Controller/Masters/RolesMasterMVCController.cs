using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_DataAccessLayer.Services.Areas.Masters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAPIArchitecture.MVC_Controller.Masters
{
    public class RolesMasterMVCController : Controller
    {
        private readonly IRoleServices _Roles;
        public RolesMasterMVCController(IRoleServices roles)
        {
            _Roles = roles;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getRolesForDropdown()
        {
            var roleList = _Roles.GetAllAsync().Result.Data.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name} ).ToList();

            return Json(roleList);
        }
    }
}

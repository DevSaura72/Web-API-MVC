using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_Framework.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WebAPIArchitecture.MVC_Controller.MVC_ViewModesl.Areas.Auth;
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Abstractions;
using MySqlX.XDevAPI.Common;
using System.Data;
using WebAPIArchitecture.MVC_Controller.MVC_ViewModesl.Areas;

namespace WebAPIArchitecture.MVC_Controller.Masters
{
    public class UserMasterMVCController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<AppUser> _userManagerServices;
        private readonly INotyfService _notyf;
        private readonly IRoleServices _roleServices;

        public UserMasterMVCController(IUserServices userServices, UserManager<AppUser> userManager, INotyfService notyf, IRoleServices roleServices)
        {
            _userServices = userServices;
            _userManagerServices = userManager;
            _notyf = notyf;
            _roleServices = roleServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllUsers(UserViewModel model)
        {
            var result = await _userServices.GetAllAsync();

            if (!result.IsSuccess || result.Data == null)
            {
                return Json(new
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<UserViewModel>()
                });
            }

            var users = result.Data.Where(x => x.IsDeleted == false).Select(user =>
            new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                IsActive = user.IsActive,
                Email = user.Email,
                ContactNumber = user.PhoneNumber,
                Roles = string.Join(",", _userManagerServices.GetRolesAsync(user).Result.ToList())
            }).ToList();


            if (!string.IsNullOrWhiteSpace(model.FirstName))
                users = users.Where(x =>
                    !string.IsNullOrEmpty(x.FirstName) &&
                    x.FirstName.Contains(model.FirstName, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            if (!string.IsNullOrWhiteSpace(model.LastName))
                users = users.Where(x =>
                    !string.IsNullOrEmpty(x.LastName) &&
                    x.LastName.Contains(model.LastName, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            if (!string.IsNullOrWhiteSpace(model.Email))
                users = users.Where(x =>
                    !string.IsNullOrEmpty(x.Email) &&
                    x.Email.Contains(model.Email, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            if (!string.IsNullOrWhiteSpace(model.ContactNumber))
                users = users.Where(x =>
                    !string.IsNullOrEmpty(x.ContactNumber) &&
                    x.ContactNumber.Contains(model.ContactNumber)
                ).ToList();

            users = users.OrderByDescending(x => x.Id).ToList();
            return Json(new
            {
                draw = 5,
                recordsTotal = users.Count,
                recordsFiltered = users.Count,
                data = users
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            string RoleForUser = "";

            if (model.UserRole != 0)
                RoleForUser = _roleServices.GetByIdAsync(model.UserRole).Result.Data.Name.ToString();

            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = "Model state not valid..!" });
            }

            if (model.Id != 0)
            {
                var user = await _userManagerServices.FindByIdAsync(model.Id.ToString());
                if (user == null)
                {
                    return Json(new { result = false, message = "User not found" });
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Email;
                user.Email = model.Email;
                user.PhoneNumber = model.ContactNumber;
                user.IsActive = model.IsActive;

                var updateResult = await _userManagerServices.UpdateAsync(user);

                var currentUserRole = await _userManagerServices.GetRolesAsync(user);

                if (currentUserRole.Any() && !currentUserRole.Contains(RoleForUser))
                {
                    foreach (var item in currentUserRole)
                    {
                        await _userManagerServices.RemoveFromRoleAsync(user, item);
                    }
                }

                if (updateResult.Succeeded && !await _userManagerServices.IsInRoleAsync(user, RoleForUser))
                {
                    var userRoleResult = await _userManagerServices.AddToRoleAsync(user, RoleForUser);
                    if (!userRoleResult.Succeeded)
                    {
                        return Json(new { result = userRoleResult.Succeeded, message = $"User updated sucesfully but, Failed to add user role" });
                    }
                }

                return Json(new
                {
                    result = updateResult.Succeeded,
                    message = updateResult.Succeeded
                        ? "User updated successfully!"
                        : updateResult.Errors.First().Description
                });
            }
            else
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.ContactNumber
                };

                string presetPassword = "Pass@123";
                var result = await _userManagerServices.CreateAsync(user, presetPassword);

                if (result.Succeeded && !await _userManagerServices.IsInRoleAsync(user, RoleForUser))
                {
                    var userRoleResult = await _userManagerServices.AddToRoleAsync(user, RoleForUser);
                    if (!userRoleResult.Succeeded)
                    {
                        return Json(new { result = userRoleResult.Succeeded, message = $"User created sucesfully but, Failed to add user role" });
                    }
                }

                return Json(new
                {
                    result = result.Succeeded,
                    message = result.Succeeded
                        ? "User added successfully!"
                        : result.Errors.First().Description
                });
            }
        }

        public async Task<IActionResult> UpdateUser(int Id)
        {
            var result = await _userServices.GetByIdAsync(Id);

            if (result.IsSuccess)
            {
                var data = new UserViewModel
                {
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    ContactNumber = result.Data.PhoneNumber,
                    Email = result.Data.Email,
                    Id = result.Data.Id,
                    IsActive = result.Data.IsActive,
                    UserRole = _roleServices.GetAllAsync().Result.Data.Where(x => x.Name == _userManagerServices.GetRolesAsync(result.Data).Result.FirstOrDefault().ToString()).FirstOrDefault().Id
                };

                return Json(new
                {
                    result = true,
                    data = data
                });
            }
            else
            {
                return Json(new { result = false, message = result.Error.Message });
            }
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userServices.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                var deleteResult = await _userServices.DelteAsync(id);
                if (deleteResult.IsSuccess)
                {

                    return Json(new
                    {
                        result = true,
                        message = "User deleted succesfuly..!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        result = false,
                        message = result.Error.ToString()
                    });
                }
            }
            else
            {
                return Json(new
                {
                    result = false,
                    message = result.Error.ToString()
                });
            }

        }


    }
}

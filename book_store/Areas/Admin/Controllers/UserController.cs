using book_store.Areas.Admin.Services;
using book_store.Areas.Admin.Services.IServices;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IApplicationUserService applicationUserService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = _applicationUserService.getApplicationUser(userId).Data,
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name, // สิ่งที่แสดงใน dropdown
                    Value = i.Id // ค่าที่ส่งไปยัง server
                })
            };

            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_applicationUserService.getApplicationUser(userId).Data).GetAwaiter().GetResult().FirstOrDefault();
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            string oldRole = _userManager.GetRolesAsync(_applicationUserService.getApplicationUser(roleManagementVM.ApplicationUser.Id).Data)
                .GetAwaiter().GetResult().FirstOrDefault();

            string newRole = _roleManager.FindByIdAsync(roleManagementVM.ApplicationUser.Role)
                            .GetAwaiter().GetResult().Name;

            ApplicationUser applicationUser = _applicationUserService.getApplicationUser(roleManagementVM.ApplicationUser.Id).Data;
            if(oldRole != newRole)
            {
                _applicationUserService.updateApplicationUser(applicationUser);

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, newRole).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> userList = _applicationUserService.getAllApplicationUser().ToList();
            foreach(var user in userList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var user = _applicationUserService.getApplicationUser(id).Data;
            if (user == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            // unlock
            if ((user.LockoutEnd != null) && (user.LockoutEnd > DateTime.Now))
            {
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddMinutes(30);
            }

            _applicationUserService.updateApplicationUser(user);
            return Json(new { success = true, message = "Operation Successful" });
        }

        #endregion
    }
}

using book_store.Areas.Admin.Services.IServices;
using book_store.Models;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        #endregion
    }
}

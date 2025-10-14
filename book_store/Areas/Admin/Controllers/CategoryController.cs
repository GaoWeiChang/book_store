using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryService.GetAllCategories().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            ServiceResult result = _categoryService.CreateCategory(category);
            if (!result.Success)
            {
                TempData["error"] = result.Message;
                return View(category);
            }

            TempData["success"] = result.Message;
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            ServiceResult<Category> result = _categoryService.GetCategoryById(id);

            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            ServiceResult result = _categoryService.UpdateCategory(category);
            if (result.Success == false)
            {
                TempData["error"] = result.Message;
                return View(category);
            }

            TempData["success"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            ServiceResult result = _categoryService.DeleteCategory(id);

            if (result.Success == false)
            {
                return Json(new { success = result.Success, message = result.Message });
            }
            return Json(new { success = result.Success, message = result.Message });
        }

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int id)
        //{
        //    ServiceResult<Category> obj = _categoryService.GetCategoryById(id);
        //    if (obj.Success == false) return NotFound();

        //    _categoryService.DeleteCategory(id);
        //    TempData["success"] = obj.Message;
        //    return RedirectToAction("Index");
        //}
    }
}

using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            ServiceResult<Category> result = _categoryService.GetCategoryById(id);
            if (result.Data == null)
            {
                return NotFound();
            }

            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            ServiceResult result = _categoryService.UpdateCategory(category);
            if (result.Success == false)
            {
                return View(category);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            ServiceResult result = _categoryService.DeleteCategory(id);

            return RedirectToAction("Index");
        }
    }
}

using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
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
            bool success = _categoryService.CreateCategory(category);
            if (!success)
            {
                return View(category);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();           
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            bool success = _categoryService.UpdateCategory(category);

            if (!success)
            {
                return View(category);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool success = _categoryService.DeleteCategory(id);
            
            return RedirectToAction("Index");
        }
    }
}

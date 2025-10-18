using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _productService.GetAllProducts().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = _categoryService.GetAllCategories().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM, List<IFormFile> files)
        {
            ServiceResult result = _productService.CreateProduct(productVM, files);
            if (!result.Success)
            {
                TempData["error"] = result.Message;
                return View(productVM);
            }

            TempData["success"] = result.Message;
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = _categoryService.GetAllCategories().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            productVM.Product = _productService.GetProductById(id, includeProperties: "ProductImages").Data;
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM productVM, List<IFormFile> files)
        {
            ServiceResult result = _productService.UpdateProduct(productVM, files);
            if (result.Success == false)
            {
                TempData["error"] = result.Message;
                return View(productVM);
            }

            TempData["success"] = result.Message;
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _productService.GetAllProducts().ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            ServiceResult result = _productService.DeleteProduct(id);

            if (result.Success == false)
            {
                return Json(new { success = result.Success, message = result.Message });
            }
            return Json(new { success = result.Success, message = result.Message });
        }
        # endregion
    }
}
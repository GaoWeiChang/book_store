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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
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

            productVM.Product = _productService.GetProductById(id).Data;
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

        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var productToBeDeleted = _productService.GetProductById(id).Data;
        //    if (productToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    string productPath = @"images\products\product-" + id;
        //    string path = Path.Combine(_webHostEnvironment.WebRootPath, productPath); // eg. ProjectName\wwwroot\images\product
        //    if (Directory.Exists(path))
        //    {
        //        string[] filePaths = Directory.GetFiles(path);
        //        foreach (string filePath in filePaths)
        //        {
        //            System.IO.File.Delete(filePath);
        //        }
        //        Directory.Delete(path);
        //    }

        //    _unitOfWork.Product.Remove(productToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}
        # endregion
    }
}
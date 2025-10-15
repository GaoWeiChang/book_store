using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _productService.GetAllProducts().ToList();
            return View(objProductList);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _productService.GetAllProducts().ToList();
            return Json(new { data = objProductList });
        }
        # endregion
    }
}
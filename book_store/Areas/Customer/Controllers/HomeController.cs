using book_store.Areas.Admin.Services.IServices;
using book_store.Areas.Customer.Services.IServices;
using book_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IProductService _productService;
        public HomeController(IHomeService homeService, IProductService productService)
        {
            _homeService = homeService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            List<Product> productList = _homeService.GetAllProducts().ToList();
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                ProductId = id,
                Product = _productService.GetProductById(id, includeProperties: "Category,ProductImages").Data, // navigation property
                Count = 1,
            };
            return View(cart);
        }
    }
}
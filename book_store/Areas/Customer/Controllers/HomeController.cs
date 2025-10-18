using book_store.Areas.Customer.Services.IServices;
using book_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        public IActionResult Index()
        {
            List<Product> productList = _homeService.GetAllProducts().ToList();
            return View(productList);
        }
    }
}

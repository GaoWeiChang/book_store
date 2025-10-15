using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

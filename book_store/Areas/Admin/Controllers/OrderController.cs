using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        public OrderController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

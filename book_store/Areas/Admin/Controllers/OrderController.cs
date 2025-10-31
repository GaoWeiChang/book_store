using book_store.Areas.Customer.Services.IServices;
using book_store.Models;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _orderService.GetAllOrderHeaders();

            switch (status)
            {
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == Roles.StatusPending || u.PaymentStatus == Roles.StatusPending);
                    break;
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == Roles.StatusPending);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == Roles.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == Roles.StatusApproved || u.PaymentStatus == Roles.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}

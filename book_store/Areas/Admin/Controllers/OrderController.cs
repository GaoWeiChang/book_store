using book_store.Areas.Admin.Services.IServices;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace book_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _orderService.GetOrderHeader(orderId).Data,
                OrderDetail = _orderService.GetAllOrderDetails(orderId)
            };

            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeader = _orderService.GetOrderHeader(OrderVM.OrderHeader.Id).Data;
            orderHeader.Name = OrderVM.OrderHeader.Name;
            orderHeader.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeader.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeader.City = OrderVM.OrderHeader.City;
            orderHeader.State = OrderVM.OrderHeader.State;
            orderHeader.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeader.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }
            ServiceResult update_result = _orderService.UpdateOrderHeader(orderHeader);

            if (update_result.Success)
            {
                TempData["success"] = update_result.Message;
            }
            else
            {
                TempData["error"] = update_result.Message;
            }

            return RedirectToAction("Details", new { orderid = orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult StartProcessing()
        {
            _orderService.UpdateStatus(OrderVM.OrderHeader.Id, Roles.StatusInProcess);
            TempData["success"] = "Order detail updated successfully.";

            return RedirectToAction("Details", new { orderid = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _orderService.GetOrderHeader(OrderVM.OrderHeader.Id).Data;
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = Roles.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if (orderHeader.PaymentStatus == Roles.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _orderService.UpdateOrderHeader(orderHeader);
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction("Details", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _orderService.GetOrderHeader(OrderVM.OrderHeader.Id).Data;

            if (orderHeader.PaymentStatus == Roles.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _orderService.UpdateStatus(orderHeader.Id, Roles.StatusCancelled, Roles.StatusRefunded);
            }
            else
            {
                _orderService.UpdateStatus(orderHeader.Id, Roles.StatusCancelled, Roles.StatusCancelled);
            }

            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction("Details", new { orderId = OrderVM.OrderHeader.Id });
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

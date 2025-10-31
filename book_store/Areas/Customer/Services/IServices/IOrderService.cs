using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;

namespace book_store.Areas.Customer.Services.IServices
{
    public interface IOrderService
    {
        public IEnumerable<OrderHeader> GetAllOrderHeaders();
        public ServiceResult AddOrderHeader(OrderHeader orderHeader);
        public ServiceResult<OrderHeader> GetOrderHeader(int id);
        public IEnumerable<OrderDetail> GetAllOrderDetails(int orderId);
        public ServiceResult AddOrderDetail(OrderDetail orderDetail);
        public ServiceResult UpdateOrderHeader(OrderHeader orderHeader);
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
        public void UpdateStatus(int id, string orderStatus, string paymentStatus);
    }
}

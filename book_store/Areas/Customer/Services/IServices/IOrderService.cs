using book_store.Models;
using book_store.Utility;

namespace book_store.Areas.Customer.Services.IServices
{
    public interface IOrderService
    {
        public ServiceResult AddOrderHeader(OrderHeader orderHeader);
        public ServiceResult<OrderHeader> GetOrderHeader(int id);
        public ServiceResult AddOrderDetail(OrderDetail orderDetail);
    }
}

using book_store.Areas.Customer.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;

namespace book_store.Areas.Customer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult AddOrderHeader(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork.OrderHeader.Add(orderHeader);
                _unitOfWork.Save();
                return ServiceResult.Ok("Added order header successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to add order header: {ex.Message}.");
            }
        }

        public ServiceResult<OrderHeader> GetOrderHeader(int id)
        {
            try
            {
                var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
                if (orderHeader == null) return ServiceResult<OrderHeader>.Fail("Id not found");

                return ServiceResult<OrderHeader>.Ok(orderHeader, "Success to get order header");
            }
            catch (Exception ex)
            {
                return ServiceResult<OrderHeader>.Fail($"Fail to retrive order header: {ex.Message}");
            }
        }

        public ServiceResult AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
                return ServiceResult.Ok("Added order detail successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to add order detail: {ex.Message}.");
            }
        }
    }
}

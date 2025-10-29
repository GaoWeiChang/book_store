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
    }
}

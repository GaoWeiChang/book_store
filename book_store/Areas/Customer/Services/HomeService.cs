using book_store.Areas.Customer.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;

namespace book_store.Areas.Customer.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
        }
    }
}

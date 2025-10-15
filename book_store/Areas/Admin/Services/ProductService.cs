using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;

namespace book_store.Areas.Admin.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll(includeProperties:"Category");
        }
    }
}

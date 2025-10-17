using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        public ServiceResult CreateProduct(ProductVM productVM, List<IFormFile> files);
    }
}

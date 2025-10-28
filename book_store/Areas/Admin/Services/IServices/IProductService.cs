using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface IProductService
    {
        public IEnumerable<Product> GetAllProducts();
        public IEnumerable<ProductImage> GetAllProductImages();
        public ServiceResult CreateProduct(ProductVM productVM, List<IFormFile> files);
        public ServiceResult<Product> GetProductById(int? productId, string? includeProperties = null, bool tracked = false);
        public ServiceResult UpdateProduct(ProductVM productVM, List<IFormFile> files);
        public ServiceResult DeleteProduct(int? id);
    }
}

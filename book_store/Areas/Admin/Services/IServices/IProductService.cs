using book_store.Models;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
    }
}

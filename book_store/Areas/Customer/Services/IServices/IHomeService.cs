using book_store.Models;

namespace book_store.Areas.Customer.Services.IServices
{
    public interface IHomeService
    {
        IEnumerable<Product> GetAllProducts();
    }
}

using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;

namespace book_store.Areas.Customer.Services.IServices
{
    public interface ICartService
    {
        IEnumerable<ShoppingCart> GetAllItemsFromCart(string? userId);
        public ServiceResult AddItemToCart(ShoppingCart shoppingCart);
        public ServiceResult<ShoppingCart> GetCartItemById(string userId, int productId);
        public ServiceResult<ShoppingCart> GetCartById(int cartId);
        public ServiceResult UpdateCart(ShoppingCart shoppingCart);
        public ServiceResult DeleteCartItem(int? cartId);
        public ServiceResult<ApplicationUser> GetUserbyId(string Id);
    }
}

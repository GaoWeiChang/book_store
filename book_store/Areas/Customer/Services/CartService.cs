using book_store.Areas.Customer.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace book_store.Areas.Customer.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult AddItemToCart(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                return ServiceResult.Ok("Added to cart.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to add item: {ex.Message}.");
            }
        }

        public IEnumerable<ShoppingCart> GetAllItemsFromCart(string? userId)
        {
            return _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
        }

        public ServiceResult<ShoppingCart> GetCartItemById(string userId, int productId)
        {
            if(productId < 0) return ServiceResult<ShoppingCart>.Fail($"Product Id must be positive.");

            try
            {
                var cart = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == productId);
                if (cart == null) return ServiceResult<ShoppingCart>.Fail("Cart not found");

                return ServiceResult<ShoppingCart>.Ok(cart, "Success to get cart");
            }
            catch (Exception ex)
            {
                return ServiceResult<ShoppingCart>.Fail($"Fail to retrive cart: {ex.Message}");
            }
        }

        public ServiceResult<ShoppingCart> GetCartById(int cartId)
        {
            try
            {
                var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
                if (cart == null) return ServiceResult<ShoppingCart>.Fail("Cart not found");

                return ServiceResult<ShoppingCart>.Ok(cart, "Success to get cart item");
            }
            catch (Exception ex)
            {
                return ServiceResult<ShoppingCart>.Fail($"Fail to retrive cart item: {ex.Message}");
            }
        }

        public ServiceResult<ApplicationUser> GetUserbyId(string userId)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                if (user == null) return ServiceResult<ApplicationUser>.Fail("User not found");

                return ServiceResult<ApplicationUser>.Ok(user, "Success to get user");
            }
            catch (Exception ex)
            {
                return ServiceResult<ApplicationUser>.Fail($"Fail to get user: {ex.Message}");
            }
        }

        public ServiceResult UpdateCart(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork.ShoppingCart.Update(shoppingCart);
                _unitOfWork.Save();
                return ServiceResult.Ok("Cart updated");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to update cart: {ex.Message}.");
            }
        }

        public ServiceResult DeleteCartItem(int? cartId)
        {
            try
            {
                var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);

                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                return ServiceResult.Ok("Cart deleted");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to delete cart: {ex.Message}");
            }
        }
    }
}

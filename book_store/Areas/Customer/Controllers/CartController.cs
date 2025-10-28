using book_store.Areas.Admin.Services.IServices;
using book_store.Areas.Customer.Services.IServices;
using book_store.Models;
using book_store.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace book_store.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        public readonly ICartService _cartService;
        public readonly IProductService _productService;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _cartService.GetAllItemsFromCart(userId),
                OrderHeader = new()
            };

            IEnumerable<ProductImage> productImage = _productService.GetAllProductImages();

            // calculate total price
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product.ProductImages = productImage.Where(u => u.ProductId == cart.Product.Id).ToList();
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

    }
}

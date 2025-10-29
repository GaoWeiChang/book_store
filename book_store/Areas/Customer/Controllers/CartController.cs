using book_store.Areas.Admin.Services.IServices;
using book_store.Areas.Customer.Services.IServices;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
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

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _cartService.GetAllItemsFromCart(userId),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _cartService.GetUserbyId(userId).Data;
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        public IActionResult IncreaseCartItem(int Id)
        {
            var cart = _cartService.GetCartById(Id).Data;
            cart.Count += 1;
            ServiceResult result = _cartService.UpdateCart(cart);

            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult DecreaseCartItem(int Id)
        {
            var cart = _cartService.GetCartById(Id).Data;
            ServiceResult result;

            if (cart.Count <= 1)
            {
                result = _cartService.DeleteCartItem(Id);
            }
            else
            {
                cart.Count -= 1;
                result = _cartService.UpdateCart(cart);
            }

            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            ServiceResult result = _cartService.DeleteCartItem(Id);

            if (result.Success == false)
            {
                return Json(new { success = result.Success, message = result.Message });
            }
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}

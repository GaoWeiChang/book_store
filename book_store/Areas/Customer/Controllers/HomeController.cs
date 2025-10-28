using book_store.Areas.Admin.Services.IServices;
using book_store.Areas.Customer.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace book_store.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IHomeService homeService, IProductService productService, ICartService cartService, IUnitOfWork unitOfWork)
        {
            _homeService = homeService;
            _productService = productService;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> productList = _homeService.GetAllProducts().ToList();
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                ProductId = productId,
                Product = _productService.GetProductById(productId, includeProperties: "Category,ProductImages").Data, // navigation property
                Count = 1,
            };

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ServiceResult<ShoppingCart> result = _cartService.GetCartItemById(userId=shoppingCart.ApplicationUserId, shoppingCart.ProductId);
            ShoppingCart cart = result.Data;

            if (cart != null)
            {
                cart.Count += shoppingCart.Count;
                _cartService.UpdateCart(cart);
            }
            else
            {
                _cartService.AddItemToCart(shoppingCart);
            }

            
            return RedirectToAction("Index");
        }
    }
}
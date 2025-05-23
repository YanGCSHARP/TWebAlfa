using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using LNP.Domain.Repositories;

namespace LNP.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService = new ProductService();
        private readonly UserContextService _userContext = new UserContextService();
        

        public CartController()
        {
            _cartService = new CartService(
                new CartRepository(), 
                new ProductRepository(),
                new UserRepository()
            );
            
            
        }

       
        public async Task<ActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);
            return View(cartItems);
        }

        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddItem(Guid id, int quantity = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _cartService.AddToCartAsync(userId, id, quantity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> UpdateQuantity(Guid itemId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _cartService.UpdateQuantityAsync(itemId, quantity, userId); 
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> RemoveItem(Guid itemId)
        {
            var userId = GetCurrentUserId();
            await _cartService.RemoveItemAsync(itemId, userId); 
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> AddAndCheckout(Guid id, int quantity = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _cartService.AddToCartAsync(userId, id, quantity);
                return RedirectToAction("Index", "Checkout");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Details", "Catalog", new { id });
            }
        }
        
        
        
        private Guid GetCurrentUserId()
        {
            return _userContext.GetCurrentUserId();
        }
    }
}
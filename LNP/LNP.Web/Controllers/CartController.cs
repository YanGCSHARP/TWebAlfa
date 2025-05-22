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
        

        public CartController()
        {
            _cartService = new CartService(
                new CartRepository(), 
                new ProductRepository(),
                new UserRepository()
            );
            
            
        }

        // Просмотр корзины
        public async Task<ActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _cartService.GetCartAsync(userId);
            return View(cartItems);
        }

        // CartController.cs
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
                // Логирование ошибки
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Обновление количества
        [HttpPost]
        public async Task<ActionResult> UpdateQuantity(Guid itemId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _cartService.UpdateQuantityAsync(itemId, quantity, userId); // Добавляем userId
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        // Удаление товара
        [HttpPost]
        public async Task<ActionResult> RemoveItem(Guid itemId)
        {
            var userId = GetCurrentUserId();
            await _cartService.RemoveItemAsync(itemId, userId); // Добавляем userId
            return RedirectToAction("Index");
        }
        
        
        // CartController.cs
        // CartController.cs
        private Guid GetCurrentUserId()
        {
            if (!User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated");

            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                throw new UnauthorizedAccessException("Authentication cookie not found");

            try
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || string.IsNullOrEmpty(ticket.UserData))
                    throw new InvalidOperationException("Invalid authentication ticket");

                var userDataParts = ticket.UserData.Split('|');
                if (userDataParts.Length < 1)
                    throw new InvalidOperationException("User data format incorrect");

                if (!Guid.TryParseExact(userDataParts[0], "N", out var userId))
                    throw new InvalidOperationException("Invalid GUID format in user data");

                return userId;
            }
            catch (Exception ex)
            {
                // Добавьте здесь логирование ошибки
                throw new InvalidOperationException("Error retrieving user data", ex);
            }
        }
    }
}
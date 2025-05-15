using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Domain.Models;

namespace TWebAlfa5.Controllers
{
    
    public class CartController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();

        // Модель элемента корзины
        

        // Корзина в сессии
        private List<CartItem> Cart
        {
            get
            {
                var cart = Session["Cart"] as List<CartItem>;
                if (cart == null)
                {
                    cart = new List<CartItem>();
                    Session["Cart"] = cart;
                }
                return cart;
            }
            set => Session["Cart"] = value;
        }

        // Добавление товара в корзину
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddToCart(Guid productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                TempData["Error"] = "Товар не найден";
                return RedirectToAction("Index", "Product");
            }

            var cart = Cart;
            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
                existingItem.Quantity += quantity;
            else
                cart.Add(new CartItem 
                { 
                    ProductId = productId, 
                    
                    Quantity = quantity, 
                    Price = product.Price,
                    Product = product
                });

            Cart = cart;
            return RedirectToAction("Details", "Product", new { id = productId });
        }

        // Просмотр корзины
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            var cart = Cart;
            foreach (var item in cart)
            {
                item.Product = db.Products.Find(item.ProductId);
            }
            return View(cart);
        }

        // Обновление количества
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult UpdateCart(Guid productId, int quantity)
        {
            var cart = Cart;
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity > 0) item.Quantity = quantity;
                else cart.Remove(item);
            }

            Cart = cart;
            return RedirectToAction("Index");
        }

        // Удаление товара
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult RemoveFromCart(Guid productId)
        {
            var cart = Cart;
            cart.RemoveAll(i => i.ProductId == productId);
            Cart = cart;
            return RedirectToAction("Index");
        }

        // Очистка корзины
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult ClearCart()
        {
            Cart.Clear();
            return RedirectToAction("Index");
        }
    }
}
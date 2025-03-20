using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TWebAlfa5.Models;
using TWebAlfa5.Controllers;

namespace TWebAlfa5.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();

        
        // Оформление заказа
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as List<CartController.CartItem>;
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Корзина пуста";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                CustomerName = User.Identity.Name, // Используем имя пользователя
                OrderDate = DateTime.Now,
                TotalPrice = cart.Sum(i => i.TotalPrice),
                OrderItems = cart.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Product = i.Product,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    
                    
                }).ToList()
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                // Сохранение заказа
                db.Orders.Add(order);
                db.SaveChanges();

                // Очистка корзины
                Session["Cart"] = new List<CartController.CartItem>();

                return RedirectToAction("Confirmation", new { id = order.Id });
            }
            return View(order);
        }

        // Подтверждение заказа
        public ActionResult Confirmation(Guid id)
        {
            var order = db.Orders
                .Include(o => o.OrderItems.Select(i => i.Product))
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
    }
}
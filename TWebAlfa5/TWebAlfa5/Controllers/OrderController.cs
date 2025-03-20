using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TWebAlfa5.Models;

namespace TWebAlfa5.Controllers
{
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();

        // GET: Order/Checkout
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Корзина пуста";
                return RedirectToAction("Index", "Cart");
            }

            // Загружаем продукты из базы данных
            var productIds = cart.Select(i => i.ProductId).ToList();
            var products = db.Products.Where(p => productIds.Contains(p.Id)).ToList();

            // Создаем заказ с UserId и Status
            var order = new Order
            {
                CustomerName = User.Identity.Name,
                UserId = User.Identity.Name, // Используем имя пользователя как UserId
                Status = OrderStatus.Pending, // Устанавливаем статус
                OrderDate = DateTime.Now,
                TotalPrice = cart.Sum(i => i.TotalPrice),
                OrderItems = new List<OrderItem>()
            };

            // Добавляем товары в заказ
            foreach (var item in cart)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Product = product,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }
            }

            return View(order);
        }

        // POST: Order/Checkout
        // Измените метод POST Checkout для передачи адреса доставки
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(Order order)
        {
            if (!HasShippingAddress())
            {
                TempData["Error"] = "Пожалуйста, добавьте адрес доставки перед оформлением заказа.";
                return RedirectToAction("AddAddress", "Profile");
            }

            if (ModelState.IsValid)
            {
                var cart = Session["Cart"] as List<CartItem>;
                if (cart == null || !cart.Any())
                {
                    TempData["Error"] = "Корзина пуста";
                    return RedirectToAction("Index", "Cart");
                }

                // Загружаем продукты из базы данных
                var productIds = cart.Select(i => i.ProductId).ToList();
                var products = db.Products.Where(p => productIds.Contains(p.Id)).ToList();

                // Проверка наличия всех товаров
                foreach (var item in cart)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null || (product.Stock.HasValue && product.Stock < item.Quantity))
                    {
                        TempData["ErrorProduct"] = product?.Name ?? "Товар";
                        return RedirectToAction("ProductUnavailable");
                    }
                }

                // Создаем заказ
                var newOrder = new Order
                {
                    CustomerName = User.Identity.Name,
                    UserId = User.Identity.Name,
                    Status = OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    TotalPrice = cart.Sum(i => i.Quantity * i.Price),
                    OrderItems = new List<OrderItem>(),
                    ShippingAddressId = db.ShippingAddresses
                        .FirstOrDefault(a => a.UserId == User.Identity.Name)?.Id

                };

                // Добавляем товары в заказ
                foreach (var item in cart)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        newOrder.OrderItems.Add(new OrderItem
                        {
                            ProductId = item.ProductId,
                            Product = product,
                            Quantity = item.Quantity,
                            Price = product.Price
                        });

                        // Уменьшаем количество товара на складе
                        if (product.Stock.HasValue)
                        {
                            product.Stock -= item.Quantity;
                        }
                    }
                }

                db.Orders.Add(newOrder);
                db.SaveChanges();

                // Очищаем корзину
                Session["Cart"] = new List<CartItem>();
                return RedirectToAction("Confirmation", new { id = newOrder.Id });
            }

            return View(order);
        }


        // Подтверждение заказа
        private bool HasShippingAddress()
        {
            var userId = User.Identity.Name;
            return db.ShippingAddresses.Any(a => a.UserId == userId);
        }

// Измените метод Confirmation следующим образом:
        public ActionResult Confirmation(Guid id)
        {
            if (!HasShippingAddress())
            {
                TempData["Error"] = "Пожалуйста, добавьте адрес доставки перед оформлением заказа.";
                return RedirectToAction("AddAddress", "Profile");
            }

            var order = db.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderItems.Select(i => i.Product))
                .Include(o => o.ShippingAddress) // Важно!

                .FirstOrDefault(o => o.Id == id);

            if (order == null) return HttpNotFound();
            return View(order);


        }

        // Мгновенный заказ - GET
        public ActionResult InstantCheckout()
        {
            var products = db.Products.ToList();
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return View();
        }

        // Мгновенный заказ - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstantCheckout(Guid productId, int quantity)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                ModelState.AddModelError("", "Продукт не найден");
                return View();
            }

            if (product.Stock.HasValue && product.Stock < quantity)
            {
                TempData["ProductName"] = product.Name;
                TempData["RequestedQuantity"] = quantity;
                TempData["AvailableQuantity"] = product.Stock;
                return RedirectToAction("ProductUnavailable");
            }

            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Неверное количество");
                return View();
            }

            // Создаем заказ
            var order = new Order
            {
                CustomerName = User.Identity.Name,
                UserId = User.Identity.Name,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                TotalPrice = product.Price * quantity,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = productId,
                        Product = product,
                        Quantity = quantity,
                        Price = product.Price
                    }
                }
            };

            // Уменьшаем количество товара на складе
            if (product.Stock.HasValue)
            {
                product.Stock -= quantity;
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return View("InstantCheckout", order);;

        }

        // Страница с информацией о недоступности товара
        public ActionResult ProductUnavailable()
        {
            return View();
        }
    }
}
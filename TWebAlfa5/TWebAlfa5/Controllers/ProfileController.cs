using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TWebAlfa5.Models;

namespace TWebAlfa5.Controllers
{
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();

        // GET: Profile/Index
        public ActionResult Index()
        {
            var userId = User.Identity.Name; // Используем имя пользователя как идентификатор
            var shippingAddress = db.ShippingAddresses.FirstOrDefault(a => a.UserId == userId);
            var orders = db.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderItems.Select(i => i.Product))
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            var model = new ProfileViewModel
            {
                ShippingAddress = shippingAddress,
                Orders = orders
            };

            return View(model);
        }

        // GET: Profile/AddAddress
        public ActionResult AddAddress()
        {
            return View();
        }

        // POST: Profile/AddAddress
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAddress(ShippingAddress model)
        {
            // Устанавливаем UserId до проверки валидации
            model.UserId = User.Identity.Name;
    
            // Удаляем ошибки валидации для UserId
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = Guid.NewGuid();
                    db.ShippingAddresses.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при сохранении адреса: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Profile/EditAddress
        public ActionResult EditAddress()
        {
            var userId = User.Identity.Name;
            var shippingAddress = db.ShippingAddresses.FirstOrDefault(a => a.UserId == userId);
            if (shippingAddress == null)
            {
                return HttpNotFound();
            }

            return View(shippingAddress);
        }

        // POST: Profile/EditAddress
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddress(ShippingAddress model)
        {
            if (ModelState.IsValid)
            {
                var existingAddress = db.ShippingAddresses.FirstOrDefault(a => a.UserId == model.UserId);
                if (existingAddress == null)
                {
                    return HttpNotFound();
                }

                existingAddress.Name = model.Name;
                existingAddress.Phone = model.Phone;
                existingAddress.Country = model.Country;
                existingAddress.City = model.City;
                existingAddress.AddressLine = model.AddressLine;
                existingAddress.PostalCode = model.PostalCode;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }

    public class ProfileViewModel
    {
        public ShippingAddress ShippingAddress { get; set; }
        public List<Order> Orders { get; set; }
    }
}
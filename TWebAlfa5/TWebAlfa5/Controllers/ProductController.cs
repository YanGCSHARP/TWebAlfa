using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TWebAlfa5.Models;
using System.Data.Entity;

namespace TWebAlfa5.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class ProductController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public ActionResult Details(Guid id)
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            // Получаем выбранный CategoryId из формы
            var categoryId = Request.Form["CategoryId"];
    
            if (!string.IsNullOrEmpty(categoryId))
            {
                // Находим категорию в базе данных
                var category = db.Categories.Find(new Guid(categoryId));
                product.Category = category;
            }

            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Если ошибка, повторно заполняем список категорий
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            return View(product);
        }

        public ActionResult Edit(Guid id)
        {
            var product = db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Передаем список категорий с выбранным значением
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.Category?.Id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                // Получаем выбранный CategoryId из формы
                var categoryId = Request.Form["CategoryId"];
        
                // Находим категорию в базе данных
                var category = db.Categories.Find(new Guid(categoryId));
        
                // Загружаем текущий продукт из базы
                var existingProduct = db.Products.Find(product.Id);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                // Обновляем поля
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.Category = category; // Обновляем категорию
                

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Если модель невалидна, повторно заполняем список категорий
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.Category?.Id);
            return View(product);
        }

        public ActionResult Delete(Guid id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // Controllers/ProductController.cs
        [AllowAnonymous] // Разрешаем доступ без авторизации
        public ActionResult Catalog()
        {
            var products = db.Products
                .Include(p => p.Category)
                 // Если есть архивные товары
                .ToList();
    
            return View(products);
        }

        
       
        
    }
}

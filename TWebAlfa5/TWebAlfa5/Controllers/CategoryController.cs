using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TWebAlfa5.Models;
using System.Data.Entity;

namespace TWebAlfa5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();

        public ActionResult Index()
        {
            
            return View(db.Categories.ToList());
        }

        public ActionResult Details(Guid id)
        {
            var category = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        public ActionResult Create()
        {;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Edit(Guid id)
        {
            var category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(Guid id)
        {
            var category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var category = db.Categories.Find(id);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

using System.Linq;
using System.Web.Mvc;
using TWebAlfa5.Models;

namespace TWebAlfa5.Controllers
{
    public class ProductController : Controller
    {
        private readonly WebDbContext db = new WebDbContext() ;
        
        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        
        // ADD NEW PRODUCT
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        
        // EDIT PRODUCT
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                return View(product);
            }
            return RedirectToAction("Index");
        }
    }
}
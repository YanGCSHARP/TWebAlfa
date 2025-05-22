using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using LNP.Core.Interfaces.Repositories;
using LNP.Domain.Repositories;

namespace LNP.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CategoryService _categoryService = new CategoryService();
        private readonly ProductService _productService = new ProductService();
        private readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        
        [ChildActionOnly]
        public async Task<ActionResult> Categories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return PartialView("_CategoriesPartial", categories);
        }

        
        [HttpGet]
        public async Task<ActionResult> Details(Guid? id) // Делаем Guid nullable
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
    
            if (product == null)
            {
                return HttpNotFound("Товар не найден");
            }

            return View(product);
        }

        public async Task<ActionResult> Index(Guid? categoryId)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var products = categoryId.HasValue 
                ? await _productService.GetProductsByCategoryAsync(categoryId.Value)
                : await _productService.GetAllProductsAsync();

            var groupedProducts = products
                .GroupBy(p => p.CategoryId)
                .Select(g => new GroupedProductsDto
                {
                    Category = categories.FirstOrDefault(c => c.Id == g.Key),
                    Products = g.ToList()
                })
                .Where(g => g.Category != null)
                .ToList();

            ViewBag.Categories = categories;
            ViewBag.GroupedProducts = groupedProducts; // Теперь это List<GroupedProductsDto>
            ViewBag.SelectedCategory = categoryId;

            return View();
        }
    }
}
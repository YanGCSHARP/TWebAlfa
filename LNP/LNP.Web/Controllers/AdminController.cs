// AdminController.cs
using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LNP.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductService _productService = new ProductService();
        private readonly CategoryService _categoryService = new CategoryService();

        // Products
        public async Task<ActionResult> Products()
        {
            
            var categories = await _categoryService.GetAllCategoriesAsync();
            // Исправлено: использование правильного метода из сервиса
            var products = await _productService.GetAllProductsAsync();
            ViewBag.Categories = categories;
            return View(products);
        }

        [HttpGet]
        public async Task<ActionResult> CreateProduct()
        {
            // Исправлено: преобразование к DTO
            ViewBag.Categories = (await _categoryService.GetAllCategoriesAsync())
                                .ConvertAll(c => new CategoryDto { Id = c.Id, Name = c.Name });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProduct(ProductDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                    return View(dto);
                }

                // Исправлено: использование асинхронного метода
                await _productService.CreateProductAsync(dto);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditProduct(Guid id)
        {
            // Исправлено: использование правильного метода
            var product = await _productService.GetProductByIdAsync(id);
            ViewBag.Categories = (await _categoryService.GetAllCategoriesAsync())
                                .ConvertAll(c => new CategoryDto { Id = c.Id, Name = c.Name });
            
            return View(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(ProductDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                    return View(dto);
                }

                // Исправлено: использование асинхронного метода
                await _productService.UpdateProductAsync(dto);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            // Исправлено: использование правильного метода
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Products");
        }

        // Категории
        public async Task<ActionResult> Categories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public ActionResult CreateCategory() => View();

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryDto dto)
        {
            try
            {
                // Исправлено: использование асинхронного метода
                await _categoryService.CreateCategoryAsync(dto);
                return RedirectToAction("Categories");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }
        
        [HttpGet]
        public async Task<ActionResult> EditCategory(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return View(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Categories");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCategory(CategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(dto);

                await _categoryService.UpdateCategoryAsync(dto);
                return RedirectToAction("Categories");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return RedirectToAction("Categories");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Categories");
            }
        }

        [HttpGet]
        public async Task<ActionResult> CategoryDetails(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return View(category);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Categories");
            }
        }

        // Остальные методы для категорий...
    }
}
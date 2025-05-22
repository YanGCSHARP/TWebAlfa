using LNP.Core.DTOs;
using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using LNP.Core.Interfaces.Services;
using LNP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LNP.BuisnessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository = new ProductRepository();
        private readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        private readonly ImageService _imageService = new ImageService();
        private readonly CategoryService _categoryService = new CategoryService();

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return MapToDto(product);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.ConvertAll(MapToDto);
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.ConvertAll(MapToDto);
        }

        public async Task CreateProductAsync(ProductDto dto)
        {
            ValidateProduct(dto);
            var imageUrl = _imageService.SaveImage(dto.ImageFile);
            
            var product = new ProductEf
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageUrl = imageUrl,
                CategoryId = dto.CategoryId
            };

            await _productRepository.CreateAsync(product);
        }

        public async Task UpdateProductAsync(ProductDto dto)
        {
            ValidateProduct(dto);
            var product = await _productRepository.GetByIdAsync(dto.Id);
            
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            if (dto.ImageFile != null)
            {
                product.ImageUrl = _imageService.SaveImage(dto.ImageFile);
            }

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        private ProductDto MapToDto(ProductEf product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                
                
            };
        }

        private void ValidateProduct(ProductDto dto)
        {
            if (dto.Price <= 0)
                throw new ArgumentException("Цена должна быть больше нуля");

            if (dto.StockQuantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");
        }
        
        
        
        public async Task<Dictionary<Guid, CategoryProductsGroup>> GetProductsGroupedByCategoryAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            return products
                .GroupBy(p => p.CategoryId)
                .ToDictionary(
                    g => g.Key,
                    g => new CategoryProductsGroup
                    {
                        Category = categories.FirstOrDefault(c => c.Id == g.Key),
                        Products = g.Select(MapToDto).ToList()
                    });
        }

        public class CategoryProductsGroup
        {
            public CategoryDto Category { get; set; }
            public List<ProductDto> Products { get; set; }
        }
    }
    
}
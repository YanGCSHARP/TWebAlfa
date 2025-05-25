// IProductService.cs
using LNP.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LNP.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task CreateProductAsync(ProductDto dto);
        Task UpdateProductAsync(ProductDto dto);
        Task DeleteProductAsync(Guid id);
        Task<ProductDto> GetProductByIdAsync(Guid id);
        
        Task<List<ProductDto>> GetProductsByCategoryAsync(Guid categoryId);
    }
}
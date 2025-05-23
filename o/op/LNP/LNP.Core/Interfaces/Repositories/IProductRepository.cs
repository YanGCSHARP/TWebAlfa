using LNP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LNP.Core.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<ProductEf> GetByIdAsync(Guid id);
        Task<List<ProductEf>> GetByCategoryAsync(Guid categoryId);
        
        Task<List<ProductEf>> GetAllAsync();
        
        Task CreateAsync(ProductEf product);
        Task UpdateAsync(ProductEf product);
        Task DeleteAsync(Guid id);
    }
}
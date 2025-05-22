using LNP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LNP.Core.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryEf> GetByIdAsync(Guid id);
        Task<List<CategoryEf>> GetAllAsync();
        Task CreateAsync(CategoryEf category);
        Task UpdateAsync(CategoryEf category);
        Task DeleteAsync(Guid id);
    }
}
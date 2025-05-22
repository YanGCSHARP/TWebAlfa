// ICategoryService.cs
using LNP.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LNP.Core.Entities;

namespace LNP.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<List<CategoryEf>> GetAllAsync();
        Task CreateCategoryAsync(CategoryDto dto);
        Task UpdateCategoryAsync(CategoryDto dto);
        Task DeleteCategoryAsync(Guid id);
        
        
    }
}
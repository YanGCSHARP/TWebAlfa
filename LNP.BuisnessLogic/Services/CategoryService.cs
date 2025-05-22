using LNP.Core.DTOs;
using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using LNP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LNP.BuisnessLogic.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository = new CategoryRepository();

        public async Task CreateCategoryAsync(CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Название категории обязательно");

            await _categoryRepository.CreateAsync(new CategoryEf { 
                Id = Guid.NewGuid(),
                Name = dto.Name 
            });
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.ConvertAll(c => new CategoryDto {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task UpdateCategoryAsync(CategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.Id);
            category.Name = dto.Name;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return new CategoryDto {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
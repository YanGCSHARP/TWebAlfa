
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using LNP.Domain;

namespace LNP.Domain.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task<CategoryEf> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<List<CategoryEf>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task CreateAsync(CategoryEf category)
        {
            category.Id = Guid.NewGuid();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryEf category)
        {
            var existing = await GetByIdAsync(category.Id);
            if (existing != null)
            {
                existing.Name = category.Name;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
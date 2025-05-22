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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task<ProductEf> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductEf>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<ProductEf>> GetAllAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Products
                    .Include(p => p.Category)
                    .ToListAsync();
            }
        }

        public async Task CreateAsync(ProductEf product)
        {
            product.Id = Guid.NewGuid();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductEf product)
        {
            var existing = await GetByIdAsync(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
                existing.ImageUrl = product.ImageUrl;
                existing.CategoryId = product.CategoryId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
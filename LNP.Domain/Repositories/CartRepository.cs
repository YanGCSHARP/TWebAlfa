using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LNP.Domain.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository()
        {
            _context = new AppDbContext();
        }

        public async Task AddItemAsync(CartItemEf item)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == item.UserId && ci.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                _context.Entry(existingItem).State = EntityState.Modified;
            }
            else
            {
                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(Guid itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<CartItemEf> GetCartItemAsync(Guid itemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Product.Category)
                .FirstOrDefaultAsync(ci => ci.Id == itemId);
        }

        public async Task UpdateQuantityAsync(Guid itemId, int quantity)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CartItemEf>> GetUserCartAsync(Guid userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Product.Category)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
using LNP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LNP.Core.Interfaces.Repositories;

namespace LNP.Domain.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task CreateOrderAsync(OrderEf order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderEf>> GetUserOrdersAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.Items.Select(i => i.Product))
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<OrderEf> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.Items.Select(i => i.Product))
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
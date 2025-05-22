using LNP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LNP.Core.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(OrderEf order);
        Task<List<OrderEf>> GetUserOrdersAsync(Guid userId);
        Task<OrderEf> GetOrderByIdAsync(Guid orderId);
    }
}
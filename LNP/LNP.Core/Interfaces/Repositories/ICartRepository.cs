using System;
using LNP.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LNP.Core.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task AddItemAsync(CartItemEf item);
        Task RemoveItemAsync(Guid itemId);
        Task UpdateQuantityAsync(Guid itemId, int quantity);
        Task<List<CartItemEf>> GetUserCartAsync(Guid userId);
        Task ClearCartAsync(Guid userId);
        
        Task<CartItemEf> GetCartItemAsync(Guid itemId);
    }
}
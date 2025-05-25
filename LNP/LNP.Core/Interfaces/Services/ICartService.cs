using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using LNP.Core.DTOs;

namespace LNP.Core.Interfaces.Services
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartAsync(Guid userId);
        Task UpdateQuantityAsync(Guid itemId, int quantity, Guid userId);
        Task AddToCartAsync(Guid userId, Guid productId, int quantity);

        Task<Guid> GetUserIdAsync(string email, string password);
        Task RemoveItemAsync(Guid itemId, Guid userId);

    }
}
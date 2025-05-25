using LNP.Core.DTOs;
using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LNP.Core.Interfaces.Services;
using LNP.Domain.Repositories;

namespace LNP.BuisnessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        

        public CartService(ICartRepository cartRepo, IProductRepository productRepo, IUserRepository userRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public async Task AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product.StockQuantity < quantity)
                throw new Exception("Недостаточно товара на складе");

            await _cartRepo.AddItemAsync(new CartItemEf
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }

        public async Task<List<CartItemDto>> GetCartAsync(Guid userId)
        {
            var items = await _cartRepo.GetUserCartAsync(userId);
            return items.ConvertAll(i => new CartItemDto
            {
                Id = i.Id, // Добавляем Id элемента корзины
                ProductId = i.ProductId,
                Name = i.Product.Name,
                Price = i.Product.Price,
                Quantity = i.Quantity,
                ImageUrl = i.Product.ImageUrl // Добавляем URL изображения
            });
        }
        
        public async Task UpdateQuantityAsync(Guid itemId, int quantity, Guid userId)
        {
            var item = await _cartRepo.GetCartItemAsync(itemId);
    
            // Проверка прав доступа
            if (item == null || item.UserId != userId)
                throw new Exception("Действие запрещено");

            var product = await _productRepo.GetByIdAsync(item.ProductId);
    
            if (product.StockQuantity < quantity)
                throw new Exception($"Доступно только {product.StockQuantity} шт.");

            await _cartRepo.UpdateQuantityAsync(itemId, quantity);
        }
        
        public async Task<Guid> GetUserIdAsync(string email, string password)
        {
            var user = await _userRepo.GetUserByCredentialsAsync(email, password);
            return user?.Id ?? Guid.Empty;
        }

        public async Task RemoveItemAsync(Guid itemId, Guid userId)
        {
            var item = await _cartRepo.GetCartItemAsync(itemId);
    
            if (item == null || item.UserId != userId)
                throw new Exception("Элемент не найден или доступ запрещен");

            await _cartRepo.RemoveItemAsync(itemId);
        }
    }
    
}
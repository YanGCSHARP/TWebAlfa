using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LNP.BuisnessLogic.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(
            IOrderRepository orderRepo, 
            ICartRepository cartRepo,
            IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<Guid> CreateOrderAsync(Guid userId, string address)
        {
            var cartItems = await _cartRepo.GetUserCartAsync(userId);
            if (cartItems.Count == 0)
                throw new Exception("Корзина пуста");

            // Обновляем остатки товаров
            foreach (var item in cartItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                product.StockQuantity -= item.Quantity;
                await _productRepo.UpdateAsync(product);
            }

            var order = new OrderEf
            {
                UserId = userId,
                ShippingAddress = address,
                Items = cartItems.Select(i => new OrderItemEf
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            await _orderRepo.CreateOrderAsync(order);
            await _cartRepo.ClearCartAsync(userId);
            return order.Id;
        }
    }
}
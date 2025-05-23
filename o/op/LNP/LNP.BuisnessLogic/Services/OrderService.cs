using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using LNP.Domain.Repositories;

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

        public async Task<Guid> CreateOrder(Guid userId)
        {
            var cartRepo = new CartRepository();
            var userRepo = new UserRepository();
            var productRepo = new ProductRepository();

            var cartItems = await cartRepo.GetUserCartAsync(userId);
            var user = await userRepo.GetByIdAsync(userId);

            var order = new OrderEf
            {
                UserId = userId,
                ShippingAddress = $"{user.Address}, {user.City}",
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                Items = cartItems.Select(i => new OrderItemEf
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            order.TotalAmount = order.Items.Sum(i => i.Price * i.Quantity);

            var orderRepo = new OrderRepository();
            await orderRepo.CreateOrderAsync(order);
            await cartRepo.ClearCartAsync(userId);

            return order.Id;
        }
    }
}
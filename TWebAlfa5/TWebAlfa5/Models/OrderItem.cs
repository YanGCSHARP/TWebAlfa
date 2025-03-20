using System;

namespace TWebAlfa5.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }  // Связь с заказом
        public Order Order { get; set; }

        public Guid ProductId { get; set; } // Связь с продуктом
        public Product Product { get; set; }

        public int Quantity { get; set; } // Количество товара
        public decimal Price { get; set; } // Цена за единицу
    }
}
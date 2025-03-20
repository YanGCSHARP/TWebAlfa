using System;

namespace TWebAlfa5.Models
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
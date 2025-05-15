
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace eUseControl.Domain.Models
{
    public class Order : BaseDbItem
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string UserId { get; set; } // Для связи с пользователем
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public virtual ShippingAddress ShippingAddress { get; set; }
        
        
        public Guid? ShippingAddressId { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
using System;
using System.Collections.Generic;
using LNP.Core.Entities;

namespace LNP.Core.Entities
{


    public class OrderEf
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public UserEf User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItemEf> Items { get; set; }
    }


    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Canceled
    }
}
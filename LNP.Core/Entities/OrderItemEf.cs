using System;
using LNP.Core.Entities;

public class OrderItemEf
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public OrderEf Order { get; set; }
    public Guid ProductId { get; set; }
    public ProductEf Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
using System;
using LNP.Core.Entities;

public class CartItemEf
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public UserEf User { get; set; }
    public Guid ProductId { get; set; }
    public ProductEf Product { get; set; }
    public int Quantity { get; set; }
}
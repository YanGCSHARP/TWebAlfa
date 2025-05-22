using System;
using LNP.Core.Entities;

namespace LNP.Core.Entities
{
    
    public class ProductEf
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryEf Category { get; set; }
    }
}
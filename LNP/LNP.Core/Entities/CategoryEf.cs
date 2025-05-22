using System;
using System.Collections.Generic;

namespace LNP.Core.Entities
{
    public class CategoryEf
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<ProductEf> Products { get; set; } // Связь один-ко-многим
    }
}
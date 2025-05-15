    using System.Collections.Generic;

    namespace eUseControl.Domain.Models
    {
        public class Product : BaseDbItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public Category Category { get; set; }
            public string ImageUrl { get; set; }
            public int? Stock { get; set; }
        }
    }
using System.Collections.Generic;

namespace eUseControl.Domain.Models
{
    public class Category : BaseDbItem
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
using System.Collections.Generic;

namespace TWebAlfa5.Models
{
    public class Category : BaseDbItem
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
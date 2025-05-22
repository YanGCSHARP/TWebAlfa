using System.Collections.Generic;

namespace LNP.Core.DTOs
{
    public class GroupedProductsDto
    {
        public CategoryDto Category { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
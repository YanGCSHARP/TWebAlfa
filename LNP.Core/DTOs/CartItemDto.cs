    using System;
    using System.ComponentModel.DataAnnotations;

    namespace LNP.Core.DTOs
    {
        public class CartItemDto
        {
            public Guid Id { get; set; }

            [Required]
            public Guid ProductId { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
            public decimal Price { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть не менее 1")]
            public int Quantity { get; set; }

            public string ImageUrl { get; set; }
            
            [Required]
            public Guid UserId { get; set; }

            public decimal TotalPrice => Price * Quantity;
        }
    }
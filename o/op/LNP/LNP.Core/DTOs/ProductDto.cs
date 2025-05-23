    // ProductDto.cs
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    namespace LNP.Core.DTOs
    {
        public class ProductDto
        {
            public Guid Id { get; set; }

            [Required(ErrorMessage = "Название обязательно")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Описание обязательно")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Цена обязательна")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
            public decimal Price { get; set; }

            [Required(ErrorMessage = "Количество на складе обязательно")]
            [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
            public int StockQuantity { get; set; }

            public HttpPostedFileBase ImageFile { get; set; }
            public string ImageUrl { get; set; }

            [Required(ErrorMessage = "Категория обязательна")]
            public Guid CategoryId { get; set; }
        }
    }
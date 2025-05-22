using System;
using System.ComponentModel.DataAnnotations;

namespace LNP.Core.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        public string Name { get; set; }
    }
}
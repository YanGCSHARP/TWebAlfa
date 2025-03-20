using System;
using System.ComponentModel.DataAnnotations;

namespace TWebAlfa5.Models
{
    public class ShippingAddress
    {
        public Guid Id { get; set; } // Уникальный идентификатор
        
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string AddressLine { get; set; }

        [Required]
        [Display(Name = "Почтовый индекс")]
        public string PostalCode { get; set; }

        // Связь с пользователем
        
        public string UserId { get; set; }
    }
}
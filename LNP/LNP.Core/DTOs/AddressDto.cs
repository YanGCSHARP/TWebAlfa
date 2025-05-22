using System.ComponentModel.DataAnnotations;

namespace LNP.Core.DTOs
{
    public class AddressDto
    {
        [Required(ErrorMessage = "Адрес обязателен")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Город обязателен")]
        public string City { get; set; }

        [Required(ErrorMessage = "Почтовый индекс обязателен")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Страна обязательна")]
        public string Country { get; set; }
    }
}
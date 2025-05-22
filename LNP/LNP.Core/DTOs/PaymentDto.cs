using System.ComponentModel.DataAnnotations;

namespace LNP.Core.DTOs
{
    public class PaymentDto
    {
        [Required, CreditCard(ErrorMessage = "Неверный номер карты")]
        public string CardNumber { get; set; }

        [Required, RegularExpression(@"^\d{3}$", ErrorMessage = "Неверный CVC")]
        public string Cvc { get; set; }

        [Required, Range(1, 12, ErrorMessage = "Неверный месяц")]
        public int ExpMonth { get; set; }

        [Required, Range(2023, 2030, ErrorMessage = "Неверный год")]
        public int ExpYear { get; set; }
    }
}
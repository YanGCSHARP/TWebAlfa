// UserProfileDto.cs

using System.ComponentModel.DataAnnotations;

namespace LNP.Core.DTOs
{
    public class UserProfileDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string PhoneNumber { get; set; }
    }
}
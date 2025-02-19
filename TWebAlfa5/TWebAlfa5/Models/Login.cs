using System.ComponentModel.DataAnnotations;

namespace TWebAlfa5.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
        
        public string Email { get; set; }
        
    }
}
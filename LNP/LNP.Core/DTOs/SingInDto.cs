using System.ComponentModel.DataAnnotations;

namespace LNP.Core.DTOs
{
    public class SignInDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
using System;
using LNP.Core.Enums;

namespace LNP.Core.Entities
{
    public class UserEf
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        public string HashPassword { get; set; }
        public bool AgreeToTerms { get; set; }
        
        public UserRole Role { get; set; }
    }
}
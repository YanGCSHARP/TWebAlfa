using System;
using System.Collections.Generic;

namespace TWebAlfa5.Models
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } 
        public ICollection<UserRole> UserRoles { get; set; }
    }
    
    
}
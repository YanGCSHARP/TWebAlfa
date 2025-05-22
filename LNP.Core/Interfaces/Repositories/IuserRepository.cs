using System;
using System.Threading.Tasks;
using LNP.Core.Entities;

namespace LNP.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEf> GetByEmailAsync(string email);
        Task<UserEf> GetByIdAsync(Guid userId); 
        Task CreateAsync(UserEf user);
        Task UpdateAsync(UserEf user);
        
        
        
        Task<UserEf> GetUserByCredentialsAsync(string email, string password);
        
    }
}
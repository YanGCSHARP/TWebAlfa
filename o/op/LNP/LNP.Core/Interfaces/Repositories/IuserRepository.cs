using System;
using System.Threading.Tasks;
using LNP.Core.DTOs;
using LNP.Core.Entities;

namespace LNP.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEf> GetByEmailAsync(string email);
        Task<UserEf> GetByIdAsync(Guid userId); // Добавляем метод
        Task CreateAsync(UserEf user);
        Task UpdateAsync(UserEf user); // Добавляем метод обновления
        
        Task UpdateAddressAsync(Guid userId, AddressDto address);
        
        Task UpdateUserAsync(UserEf user);
        Task<UserEf> GetUserByCredentialsAsync(string email, string password);
        
    }
}
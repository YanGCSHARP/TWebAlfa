// IUserRepository.cs
using System.Threading.Tasks;
using LNP.Core.Entities;

namespace LNP.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEf> GetByEmailAsync(string email);
        Task CreateAsync(UserEf user);
    }
}
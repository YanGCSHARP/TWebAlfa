using LNP.Core.Entities;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using LNP.Core.Interfaces.Repositories;

namespace LNP.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task<UserEf> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Добавляем реализацию GetByIdAsync
        public async Task<UserEf> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task CreateAsync(UserEf user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Добавляем метод обновления
        public async Task UpdateAsync(UserEf user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        public async Task<UserEf> GetUserByCredentialsAsync(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.HashPassword == password);
        }
        
        
    }
}
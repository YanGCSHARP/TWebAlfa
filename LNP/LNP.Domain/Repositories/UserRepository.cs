using System;
using System.Data.Entity;
using System.Threading.Tasks;
using LNP.Core.Entities;
using LNP.Core.Interfaces;
using LNP.Core.Interfaces.Repositories;
using LNP.Domain;

namespace LNP.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        
        public UserRepository()
        {
            _context = new AppDbContext(); // Убедитесь, что AppDbContext имеет конструктор без параметров
        }

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserEf> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(UserEf user)
        {
            user.Id = Guid.NewGuid(); // Убедитесь, что Id генерируется
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
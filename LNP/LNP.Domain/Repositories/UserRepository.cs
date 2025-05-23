using LNP.Core.Entities;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using LNP.Core.DTOs;
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

       
        public async Task<UserEf> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task CreateAsync(UserEf user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        
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
        
        public async Task UpdateAddressAsync(Guid userId, AddressDto address)
        {
            using (var context = new AppDbContext())
            {
                var user = await context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.Address = address.Address;
                    user.City = address.City;
                    user.PostalCode = address.PostalCode;
                    user.Country = address.Country;
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task UpdateUserAsync(UserEf user)
        {
            using (var context = new AppDbContext())
            {
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
            
        
        
    }
}
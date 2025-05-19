using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LNP.BuisnessLogic.Helper;
using LNP.Core.DTOs;
using LNP.Core.Entities;
using LNP.Core.Interfaces;
using LNP.Core.Interfaces.Repositories;
using LNP.Core.Interfaces.Services;
using LNP.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LNP.BuisnessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly Hasher _hasher;
        
        public AuthService()
        {
            _userRepo = new UserRepository(); // Убедитесь, что UserRepository имеет конструктор без параметров
            _hasher = new Hasher();
        }

        public AuthService(IUserRepository userRepo, Hasher hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        public async Task<bool> SignUpAsync(SignUpDto dto)
        {
            if (dto.Password != dto.RepeatPassword)
                return false;

            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                return false;

            var user = new UserEf
            {
                Id = Guid.NewGuid(), // Генерация Guid
                Name = dto.Name,
                Email = dto.Email,
                HashPassword = _hasher.HashPassword(dto.Password),
                AgreeToTerms = dto.AgreeToTerms,
                Role = dto.Role // Установка роли
            };

            await _userRepo.CreateAsync(user);
            return true;
        }

        public async Task<bool> SignInAsync(SignInDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            return user != null && _hasher.VerifyPassword(dto.Password, user.HashPassword);
        }
        
        public string GenerateJwtToken(UserEf user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["JwtSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()) // Добавление роли в токен
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
// IAuthService.cs
using LNP.Core.DTOs;
using System.Threading.Tasks;

namespace LNP.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> SignUpAsync(SignUpDto dto);
        Task<bool> SignInAsync(SignInDto dto);
    }
}
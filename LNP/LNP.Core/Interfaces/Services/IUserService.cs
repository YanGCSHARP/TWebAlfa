using System;
using System.Threading.Tasks;
using LNP.Core.DTOs;


namespace LNP.Core.Interfaces.Services
{
    public interface IUserService
    { 
         Guid GetCurrentUserId();
    }
}
using LNP.Core.DTOs;
using System.Threading.Tasks;

namespace LNP.BuisnessLogic.Services
{
    public class PaymentService
    {
        public Task<bool> ProcessPayment(PaymentDto dto)
        {
            return Task.FromResult(true);
        }
    }
}
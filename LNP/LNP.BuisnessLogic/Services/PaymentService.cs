using LNP.Core.DTOs;
using System.Threading.Tasks;

namespace LNP.BuisnessLogic.Services
{
    public class PaymentService
    {
        public Task<bool> ProcessPayment(PaymentDto dto)
        {
            // Заглушка для имитации успешной оплаты
            return Task.FromResult(true);
            
            // Для более реалистичной симуляции можно добавить:
            // if (dto.CardNumber == "4111111111111111") return true;
            // return false;
        }
    }
}
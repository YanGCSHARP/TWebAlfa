using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using LNP.Core.Interfaces.Repositories;
using LNP.Domain.Repositories;

namespace LNP.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly OrderService _orderService;
        private readonly PaymentService _paymentService;
        private readonly IUserRepository _userRepo;

        public CheckoutController()
        {
            _orderService = new OrderService(
                new OrderRepository(),
                new CartRepository(),
                new ProductRepository()
            );
            _paymentService = new PaymentService();
            _userRepo = new UserRepository();
        }

        [HttpPost]
        public async Task<ActionResult> ProcessPayment(PaymentDto dto)
        {
            if (!ModelState.IsValid)
                return View("Payment", dto);

            var userId = (Guid)Session["UserId"];
            var user = await _userRepo.GetByIdAsync(userId);
            
            try
            {
                // Симуляция оплаты
                var paymentResult = await _paymentService.ProcessPayment(dto);
                
                if (!paymentResult)
                {
                    ModelState.AddModelError("", "Оплата не прошла");
                    return View("Payment", dto);
                }

                var orderId = await _orderService.CreateOrderAsync(userId, user.Address);
                return RedirectToAction("OrderConfirmation", new { id = orderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Payment", dto);
            }
        }
    }
}
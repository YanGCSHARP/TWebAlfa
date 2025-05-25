using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LNP.Core.Interfaces.Repositories;
using LNP.Core.Interfaces.Services;
using LNP.Domain.Repositories;

namespace LNP.Web.Controllers
{
    [Authorize]
    
    public class CheckoutController : Controller
    {
        private readonly IUserService _userService = new UserService();
        public async Task<ActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var userRepo = new UserRepository();
            var user = await userRepo.GetByIdAsync(userId);
            

            if (string.IsNullOrEmpty(user.Address))
            {
                return RedirectToAction("Address");
            }
            

            var cartRepo = new CartRepository();
            var cartItems = await cartRepo.GetUserCartAsync(userId);
        
            ViewBag.Total = cartItems.Sum(i => i.Quantity * i.Product.Price);
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Address()
        {
            var userId = GetCurrentUserId();
            var userRepo = new UserRepository();
            var user = await userRepo.GetByIdAsync(userId);
        
            var model = new AddressDto {
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                Country = user.Country
            };
        
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Address(AddressDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userId = GetCurrentUserId();
            var userRepo = new UserRepository();
            await userRepo.UpdateAddressAsync(userId, dto);
        
            return RedirectToAction("Index");
        }

        private Guid GetCurrentUserId()
        {
            return _userService.GetCurrentUserId();
        }
        [HttpPost]
        public async Task<ActionResult> ProcessPayment(PaymentDto dto)
        {
            var orderService = new OrderService( 
                new OrderRepository(), 
                new CartRepository(),
                new ProductRepository()
            );
            var paymentService = new PaymentService();
            
            try
            {
                var userId = GetCurrentUserId();
                var orderId = await orderService.CreateOrder(userId);
                var paymentResult = await paymentService.ProcessPayment(dto);
        
                if(paymentResult)
                    return RedirectToAction("Confirmation", "Checkout", new { id = orderId });
        
                ModelState.AddModelError("", "Ошибка оплаты");
                return View("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }
        }
        public ActionResult Confirmation(Guid id)
        {
            return View(id);
        }
    }
    
    
}
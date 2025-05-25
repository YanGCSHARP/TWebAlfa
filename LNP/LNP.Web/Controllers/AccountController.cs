using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using LNP.Core.Entities;
using LNP.Core.Interfaces.Repositories;
using LNP.Core.Interfaces.Services;
using LNP.Domain.Repositories;

namespace LNP.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService = new AuthService();
        private readonly IUserRepository _userRepo = new UserRepository();
        private readonly IUserService _userService = new UserService();
        private readonly IOrderRepository _orderRepo = new OrderRepository(); 
        
        [HttpGet]
        public ActionResult SignIn() => View();
        
        [HttpGet]
        public ActionResult SignUp() => View();
        
        
        
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInDto signInDto)
        {
            if (!ModelState.IsValid)
                return View(signInDto);

            bool isAuthenticated = await _authService.SignInAsync(signInDto);
            
    
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверный email или пароль");
            return View(signInDto);
        }

        

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authService.SignUpAsync(dto);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            
            
        }
        
        public async Task<ActionResult> OrderHistory()
        {
            var userId = GetCurrentUserId();
            var orderRepo = new OrderRepository();
            var orders = await orderRepo.GetUserOrdersAsync(userId);
            return PartialView("OrderHistory", orders);
        }
        
        

        private Guid GetCurrentUserId()
        {
            return _userService.GetCurrentUserId();
        }
        
        public ActionResult Logout()
        {
            
            FormsAuthentication.SignOut();

            
            Session.Clear();

            return RedirectToAction("Index", "Home"); 
        }
        
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var userId = GetCurrentUserId();
    
            // Загружаем пользователя и заказы параллельно
            var userTask = _userRepo.GetByIdAsync(userId);
            var ordersTask = _orderRepo.GetUserOrdersAsync(userId); // Используем существующий метод
    
            await Task.WhenAll(userTask, ordersTask);
    
            ViewBag.User = await userTask;
            ViewBag.Orders = await ordersTask;
    
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Profile(UserEf model)
        {
            if (ModelState.IsValid)
            {
                await _userRepo.UpdateAsync(model);
                return RedirectToAction("Profile");
            }
            return View(model);
        }
        
        [Authorize]
        public async Task<ActionResult> EditAddress()
        {
            var userId = GetCurrentUserId();
            var user = await _userRepo.GetByIdAsync(userId);
    
            var model = new AddressDto
            {
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                Country = user.Country
            };
    
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddress(AddressDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userId = GetCurrentUserId();
            await _userRepo.UpdateAddressAsync(userId, dto);
    
            TempData["SuccessMessage"] = "Адрес успешно обновлён";
            return RedirectToAction("Profile");
        }
        
        [Authorize]
        public async Task<ActionResult> EditProfile()
        {
            var userId = GetCurrentUserId();
            var user = await _userRepo.GetByIdAsync(userId);
    
            var model = new UserProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
    
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(UserProfileDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userId = GetCurrentUserId();
            var user = await _userRepo.GetByIdAsync(userId);
    
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(dto);
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
    
            await _userRepo.UpdateUserAsync(user);
    
            TempData["SuccessMessage"] = "Профиль успешно обновлён";
            return RedirectToAction("Profile");
        }
        
        
        
    }
}
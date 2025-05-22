using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using LNP.BuisnessLogic.Services;
using LNP.Core.DTOs;
using LNP.Core.Interfaces.Repositories;
using LNP.Core.Interfaces.Services;
using LNP.Domain.Repositories;

namespace LNP.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService = new AuthService();
        private readonly IUserRepository _userRepo = new UserRepository();

        
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
        
        public ActionResult Logout()
        {
            // Если используется Forms Authentication:
            FormsAuthentication.SignOut();

            // Очистка сессии (опционально)
            Session.Clear();

            return RedirectToAction("Index", "Home"); // Перенаправление на главную страницу
        }
    }
}
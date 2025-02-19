using System;
using System.Linq;
using System.Web.Mvc;
using TWebAlfa5.Models;
using System.Web.Security;

namespace TWebAlfa5.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebDbContext db = new WebDbContext(); 

        // Действие для отображения страницы входа
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // Действие для обработки данных входа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Получение пользователя из базы данных
                var user = db.Users.FirstOrDefault(u => u.Name == model.Name);
                if (user != null)
                {
                    // Сравнение хешированного пароля
                    if (VerifyPassword(model.Password, user.PasswordHash))
                    {
                        // Установка аутентификационной куки
                        FormsAuthentication.SetAuthCookie(user.Name, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                }
            }

            // Если мы попали сюда, значит произошла ошибка, повторно отображаем форму
            return View(model);
        }


        // Действие для выхода из системы
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        

        // Действие для отображения страницы регистрации
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // Действие для обработки данных регистрации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Проверка на существование пользователя с таким же email
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует.");
                    return View(model);
                }

                // Создание нового пользователя
                var user = new User
                {
                    Email = model.Email,
                    Name = model.Email, // Или другое поле, если у вас есть отдельное поле для имени
                    PasswordHash = HashPassword(model.Password)
                };
                var role = db.Roles.FirstOrDefault(r => r.Name == "User");
                var UserRole = new UserRole
                {
                    User = user,
                    Role = role,
                    UserId = user.Id,
                    RoleId = role.Id
                    
                };
                db.UserRoles.Add(UserRole);
                db.Users.Add(user);
                db.SaveChanges();

                
                
                

                // Аутентификация пользователя
                FormsAuthentication.SetAuthCookie(user.Name, false /* createPersistentCookie */);
                return RedirectToAction("Index", "Home");
            }

            // Если мы попали сюда, значит произошла ошибка, повторно отображаем форму
            return View(model);
        }

        // Метод для хеширования пароля (используйте более надежный метод в реальных приложениях)
        private string HashPassword(string password)
        {
            // Здесь должен быть надежный алгоритм хеширования, например, bcrypt или PBKDF2
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
        }
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Получение хеша введенного пароля
            string enteredHash = FormsAuthentication.HashPasswordForStoringInConfigFile(enteredPassword, "SHA1");

            // Сравнение хешей
            return string.Equals(enteredHash, storedHash, StringComparison.Ordinal);
        }

    }
    
}

//using System;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using TWebAlfa5.Models;

//namespace TWebAlfa5.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly WebDbContext db = new WebDbContext(); 

//        [HttpGet]
//        public ActionResult Login()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Login(Login model, string returnUrl)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = db.Users.FirstOrDefault(u => u.Name == model.Name);
//                if (user != null && VerifyPassword(model.Password, user.PasswordHash))
//                {
//                    // Загружаем роли пользователя
//                    string roles = GetUserRoles(user.Id);

//                    // Создаем ticket аутентификации с ролями
//                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
//                        1, user.Name, DateTime.Now, DateTime.Now.AddHours(1), model.RememberMe, roles);

//                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
//                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
//                    Response.Cookies.Add(authCookie);

//                    if (Url.IsLocalUrl(returnUrl))
//                        return Redirect(returnUrl);

//                    return RedirectToAction("Index", "Home");
//                }

//                ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
//            }

//            return View(model);
//        }

//        public ActionResult Logout()
//        {
//            FormsAuthentication.SignOut();
//            return RedirectToAction("Login", "Account");
//        }

//        [HttpGet]
//        public ActionResult Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Register(Register model)
//        {
//            if (ModelState.IsValid)
//            {
//                if (db.Users.Any(u => u.Email == model.Email))
//                {
//                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует.");
//                    return View(model);
//                }

//                var user = new User
//                {
//                    Email = model.Email,
//                    Name = model.Name,
//                    PasswordHash = HashPassword(model.Password)
//                };

//                db.Users.Add(user);
//                db.SaveChanges();

//                // Добавляем пользователя в роль "User"
//                var role = db.Roles.FirstOrDefault(r => r.Name == "User");
//                if (role != null)
//                {
//                    db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
//                    db.SaveChanges();
//                }

//                // Аутентифицируем пользователя
//                FormsAuthentication.SetAuthCookie(user.Name, false);
//                return RedirectToAction("Index", "Home");
//            }

//            return View(model);
//        }

//        private string GetUserRoles(Guid userId)
//        {
//            var roles = db.UserRoles
//                          .Where(ur => ur.UserId == userId)
//                          .Select(ur => ur.Role.Name)
//                          .ToList();

//            return string.Join(",", roles);
//        }

//        private string HashPassword(string password)
//        {
//            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
//        }

//        private bool VerifyPassword(string enteredPassword, string storedHash)
//        {
//            string enteredHash = FormsAuthentication.HashPasswordForStoringInConfigFile(enteredPassword, "SHA1");
//            return string.Equals(enteredHash, storedHash, StringComparison.Ordinal);
//        }
//    }
//}

using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TWebAlfa5.Models;

namespace TWebAlfa5.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebDbContext db = new WebDbContext();
        
     

        // GET: Отображает форму входа
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Обрабатывает вход пользователя
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от подделки запросов
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid) // Проверяем, валидны ли данные формы
            {
                var user = db.Users.FirstOrDefault(u => u.Name == model.Name); // Ищем пользователя по имени
                if (user != null && VerifyPassword(model.Password, user.PasswordHash)) // Проверяем, существует ли пользователь и верен ли пароль
                {
                    // Получаем роли пользователя
                    string roles = GetUserRoles(user.Id);

                    // Создаём билет аутентификации с ролями, действующий 1 час
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, user.Name, DateTime.Now, DateTime.Now.AddHours(1), model.RememberMe, roles);

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket); // Шифруем билет
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket); // Создаём куку
                    Response.Cookies.Add(authCookie); // Добавляем куку в ответ

                    if (Url.IsLocalUrl(returnUrl)) // Если указан URL для возврата, перенаправляем туда
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home"); // Иначе перенаправляем на главную страницу
                }

                ModelState.AddModelError("", "Неверное имя пользователя или пароль."); // Ошибка, если данные неверны
            }

            return View(model); // Возвращаем вид с моделью для повторного ввода
        }

        // GET: Отображает форму регистрации
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Обрабатывает регистрацию нового пользователя
         // Защита от подделки запросов
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует.");
                    return View(model);
                }

                // Создаем пользователя
                var user = new User
                {
                    Email = model.Email,
                    Name = model.Name,
                    PasswordHash = HashPassword(model.Password)
                };

                db.Users.Add(user);
                db.SaveChanges(); // Сохраняем пользователя

                // Проверяем существование роли "User" и создаем, если отсутствует
                var role = db.Roles.FirstOrDefault(r => r.Name == "User");
                if (role == null)
                {
                    role = new Role { Name = "User" };
                    db.Roles.Add(role);
                    db.SaveChanges(); // Сохраняем роль
                }

                // Назначаем роль пользователю
                db.UserRoles.Add(new UserRole 
                { 
                    UserId = user.Id, 
                    RoleId = role.Id 
                });
                db.SaveChanges();

                // Создаем аутентификационный билет с ролями
                string roles = "User";
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    user.Name,
                    DateTime.Now,
                    DateTime.Now.AddHours(1),
                    false,
                    roles);

                // Шифруем билет и создаем куки
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, 
                    encryptedTicket
                );
                Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // Выход пользователя из системы
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); // Завершаем сессию пользователя
            return RedirectToAction("Login", "Account"); // Перенаправляем на страницу входа
        }

        // Вспомогательный метод: Получает роли пользователя
        private string GetUserRoles(Guid userId)
        {
            var roles = db.UserRoles
                          .Where(ur => ur.UserId == userId) // Фильтруем роли по ID пользователя
                          .Select(ur => ur.Role.Name) // Получаем названия ролей
                          .ToList();

            return string.Join(",", roles); // Объединяем роли в строку, разделённую запятыми
        }

        // Вспомогательный метод: Хеширует пароль для хранения
        private string HashPassword(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1"); // Хешируем пароль с использованием SHA1
        }

        // Вспомогательный метод: Проверяет, совпадает ли введённый пароль с хешем
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = FormsAuthentication.HashPasswordForStoringInConfigFile(enteredPassword, "SHA1"); // Хешируем введённый пароль
            return string.Equals(enteredHash, storedHash, StringComparison.Ordinal); // Сравниваем хеши
        }
    }
}
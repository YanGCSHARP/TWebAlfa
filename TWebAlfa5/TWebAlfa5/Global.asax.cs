using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using TWebAlfa5.Models;

namespace TWebAlfa5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CheckAndCreateAdmin();
            CheckAndCreateRoles();
        }
        
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    // Разделяем роли, если их несколько
                    string[] roles = authTicket.UserData.Split(',');

                    HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
        }



        private void CheckAndCreateRoles()
        {
            using (var db = new WebDbContext())
            {
                if (!db.Roles.Any())
                {
                    db.Roles.Add(new Role { Name = "Admin" });
                    db.Roles.Add(new Role { Name = "User" });
                    db.SaveChanges();
                }
            }
        }


        private void CheckAndCreateAdmin()
        {
            using (var db = new WebDbContext())
            {
                if (!db.Users.Any(u => u.Name == "Admin"))
                {
                    var admin = new User
                    {
                        Name = "Admin",
                        PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile("Admin", "SHA1"),
                        Email = "admin@localhost",
                    };
                    
                    
                    db.Users.Add(admin);
                    db.SaveChanges();
                    
                    var role = db.Roles.FirstOrDefault(r => r.Name == "Admin");
                    if (role != null)
                    {
                        db.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = role.Id });
                        db.SaveChanges();
                    }   

                }
            }
        }
        
        // В Global.asax.cs
        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 401)
            {
                context.Response.Redirect("~/Account/Login?returnUrl=" + context.Request.Url.PathAndQuery);
            }
            if (context.Response.StatusCode == 403)
            {
                context.Response.Redirect("~/Home/AccessDenied");
            }
        }

// Создайте контроллер для обработки ошибок:
        public class HomeController : Controller
        {
            public ActionResult AccessDenied()
            {
                return View();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using LNP.BuisnessLogic.Helper;
using LNP.Domain;
using LNP.BuisnessLogic.Services;
using LNP.Core.Entities;
using LNP.Core.Enums;
using LNP.Domain.Repositories;

namespace LNP.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SeedAdminUser();
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            
                    // Исправляем разделитель на "|"
                    var userData = authTicket.UserData.Split('|');
                    var roles = userData.Length > 1 ? new[] { userData[1] } : new string[0];
            
                    var identity = new FormsIdentity(authTicket);
                    var principal = new GenericPrincipal(identity, roles);
            
                    HttpContext.Current.User = principal;
                    Thread.CurrentPrincipal = principal;
                }
                catch
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect("~/Account/SignIn");
                }
            }
        }
        private void SeedAdminUser()
        {
            using (var context = new AppDbContext())
            {
                if (!context.Users.Any(u => u.Role == UserRole.Admin))
                {
                    var admin = new UserEf
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        Email = "admin@example.com",
                        HashPassword = new Hasher().HashPassword("admin123"),
                        Role = UserRole.Admin,
                        AgreeToTerms = true
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }
            }
        }
        
        
    }
}
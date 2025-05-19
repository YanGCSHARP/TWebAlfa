using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
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
        
        private void SeedAdminUser()
        {
            using (var context = new AppDbContext())
            {
                var adminEmail = "admin@example.com";
                var existingAdmin = context.Users.FirstOrDefault(u => u.Email == adminEmail);
                if (existingAdmin == null)
                {
                    var hasher = new Hasher();
                    var admin = new UserEf
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        Email = adminEmail,
                        HashPassword = hasher.HashPassword("admin123"), // Хэшируйте пароль
                        Role = UserRole.Admin // Убедитесь, что enum UserRole содержит Admin
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }
            }
        }
        
        
    }
}
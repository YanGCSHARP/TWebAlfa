using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
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
                        PasswordHash = Crypto.HashPassword("Admin"),
                        Email = "admin@localhost",
                    };
                    db.Users.Add(admin);
                    db.SaveChanges();

                }
            }
        }
    }
}
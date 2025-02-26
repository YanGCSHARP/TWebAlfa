﻿    using System.Data.Entity;
    using TWebAlfa5.Models;

    namespace TWebAlfa5
    {
        public class WebDbContext : DbContext
        {
            public WebDbContext() : base("name=TWebDbContext")
            {
            }
            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<UserRole> UserRoles { get; set; }
            public DbSet<Product> Products { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.HasDefaultSchema("public");
                
                modelBuilder.Entity<UserRole>()
                    .HasKey(ur => new { ur.UserId, ur.RoleId });
                
            }
        }
    }
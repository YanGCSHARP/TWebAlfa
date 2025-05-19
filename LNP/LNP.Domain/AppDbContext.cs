using System.Data.Entity;
using LNP.Core.Entities;

namespace LNP.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext") { }

        public DbSet<UserEf> Users { get; set; }
        
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Конфигурация для PostgreSQL
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
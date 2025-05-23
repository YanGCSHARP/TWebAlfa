using System.Data.Entity;
using LNP.Core.Entities;

namespace LNP.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext") { }

        public DbSet<UserEf> Users { get; set; }
        public DbSet<CategoryEf> Categories { get; set; }
        public DbSet<ProductEf> Products { get; set; }
        public DbSet<CartItemEf> CartItems { get; set; }
        public DbSet<OrderEf> Orders { get; set; }
        public DbSet<OrderItemEf> OrderItems { get; set; }
        public DbSet<PaymentEf> Payments { get; set; }  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Конфигурация для PostgreSQL
            modelBuilder.HasDefaultSchema("public");
            
            modelBuilder.Entity<CartItemEf>()
                .HasRequired(ci => ci.User)
                .WithMany()
                .HasForeignKey(ci => ci.UserId);

            modelBuilder.Entity<CartItemEf>()
                .HasRequired(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);
            
            modelBuilder.Entity<ProductEf>()
                .HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
                
            // Добавляем конфигурацию для PaymentEf
            modelBuilder.Entity<PaymentEf>()
                .HasRequired(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId);
        }
    }
}
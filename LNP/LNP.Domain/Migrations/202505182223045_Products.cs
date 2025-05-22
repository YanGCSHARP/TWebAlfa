namespace LNP.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Products : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.CartItemEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.ProductEfs", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("public.UserEfs", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "public.ProductEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockQuantity = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        CategoryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.CategoryEfs", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "public.CategoryEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.OrderItemEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.OrderEfs", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("public.ProductEfs", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "public.OrderEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingAddress = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.UserEfs", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.OrderItemEfs", "ProductId", "public.ProductEfs");
            DropForeignKey("public.OrderEfs", "UserId", "public.UserEfs");
            DropForeignKey("public.OrderItemEfs", "OrderId", "public.OrderEfs");
            DropForeignKey("public.CartItemEfs", "UserId", "public.UserEfs");
            DropForeignKey("public.CartItemEfs", "ProductId", "public.ProductEfs");
            DropForeignKey("public.ProductEfs", "CategoryId", "public.CategoryEfs");
            DropIndex("public.OrderEfs", new[] { "UserId" });
            DropIndex("public.OrderItemEfs", new[] { "ProductId" });
            DropIndex("public.OrderItemEfs", new[] { "OrderId" });
            DropIndex("public.ProductEfs", new[] { "CategoryId" });
            DropIndex("public.CartItemEfs", new[] { "ProductId" });
            DropIndex("public.CartItemEfs", new[] { "UserId" });
            DropTable("public.OrderEfs");
            DropTable("public.OrderItemEfs");
            DropTable("public.CategoryEfs");
            DropTable("public.ProductEfs");
            DropTable("public.CartItemEfs");
        }
    }
}

namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.OrderItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("public.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "public.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerName = c.String(),
                        CustomerEmail = c.String(),
                        CustomerPhone = c.String(),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.OrderItems", "ProductId", "public.Products");
            DropForeignKey("public.OrderItems", "OrderId", "public.Orders");
            DropIndex("public.OrderItems", new[] { "ProductId" });
            DropIndex("public.OrderItems", new[] { "OrderId" });
            DropTable("public.Orders");
            DropTable("public.OrderItems");
        }
    }
}

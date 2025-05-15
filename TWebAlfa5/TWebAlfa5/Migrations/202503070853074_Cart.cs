namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cart : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Products", "ImageUrl", c => c.String());
            AddColumn("public.Products", "Stock", c => c.Int());
            AddColumn("public.Orders", "OrderDate", c => c.DateTime(nullable: false));
            AddColumn("public.Orders", "UserId", c => c.String());
            AddColumn("public.Orders", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.Orders", "Status");
            DropColumn("public.Orders", "UserId");
            DropColumn("public.Orders", "OrderDate");
            DropColumn("public.Products", "Stock");
            DropColumn("public.Products", "ImageUrl");
        }
    }
}

namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.ShippingAddresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        City = c.String(nullable: false),
                        AddressLine = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("public.Orders", "ShippingAddress_Id", c => c.Guid());
            CreateIndex("public.Orders", "ShippingAddress_Id");
            AddForeignKey("public.Orders", "ShippingAddress_Id", "public.ShippingAddresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.Orders", "ShippingAddress_Id", "public.ShippingAddresses");
            DropIndex("public.Orders", new[] { "ShippingAddress_Id" });
            DropColumn("public.Orders", "ShippingAddress_Id");
            DropTable("public.ShippingAddresses");
        }
    }
}

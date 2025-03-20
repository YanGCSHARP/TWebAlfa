namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipping5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("public.Orders", new[] { "ShippingAddressId" });
            AlterColumn("public.Orders", "ShippingAddressId", c => c.Guid(nullable: true));
            CreateIndex("public.Orders", "ShippingAddressId");
        }
        
        public override void Down()
        {
            DropIndex("public.Orders", new[] { "ShippingAddressId" });
            AlterColumn("public.Orders", "ShippingAddressId", c => c.Guid());
            CreateIndex("public.Orders", "ShippingAddressId");
        }
    }
}

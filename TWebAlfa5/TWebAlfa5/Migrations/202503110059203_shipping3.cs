namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipping3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "public.Orders", name: "ShippingAddress_Id", newName: "ShippingAddressId");
            RenameIndex(table: "public.Orders", name: "IX_ShippingAddress_Id", newName: "IX_ShippingAddressId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "public.Orders", name: "IX_ShippingAddressId", newName: "IX_ShippingAddress_Id");
            RenameColumn(table: "public.Orders", name: "ShippingAddressId", newName: "ShippingAddress_Id");
        }
    }
}

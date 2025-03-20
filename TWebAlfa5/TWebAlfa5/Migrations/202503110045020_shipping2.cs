namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipping2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.ShippingAddresses", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("public.ShippingAddresses", "UserId", c => c.String(nullable: false));
        }
    }
}

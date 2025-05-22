namespace LNP.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.PaymentEfs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(),
                        TransactionId = c.String(),
                        IsSuccessful = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.OrderEfs", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            AddColumn("public.UserEfs", "Address", c => c.String());
            AddColumn("public.UserEfs", "City", c => c.String());
            AddColumn("public.UserEfs", "PostalCode", c => c.String());
            AddColumn("public.UserEfs", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("public.PaymentEfs", "OrderId", "public.OrderEfs");
            DropIndex("public.PaymentEfs", new[] { "OrderId" });
            DropColumn("public.UserEfs", "Country");
            DropColumn("public.UserEfs", "PostalCode");
            DropColumn("public.UserEfs", "City");
            DropColumn("public.UserEfs", "Address");
            DropTable("public.PaymentEfs");
        }
    }
}

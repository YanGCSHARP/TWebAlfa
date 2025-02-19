namespace TWebAlfa5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Category_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "public.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.UserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("public.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("public.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "public.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.UserRoles", "UserId", "public.Users");
            DropForeignKey("public.UserRoles", "RoleId", "public.Roles");
            DropForeignKey("public.Products", "Category_Id", "public.Categories");
            DropIndex("public.UserRoles", new[] { "RoleId" });
            DropIndex("public.UserRoles", new[] { "UserId" });
            DropIndex("public.Products", new[] { "Category_Id" });
            DropTable("public.Users");
            DropTable("public.UserRoles");
            DropTable("public.Roles");
            DropTable("public.Categories");
            DropTable("public.Products");
        }
    }
}

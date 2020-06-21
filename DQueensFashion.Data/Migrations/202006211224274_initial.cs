namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            AddColumn("dbo.LineItems", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LineItems", "Description", c => c.String());
            AddColumn("dbo.Products", "ExtraSmallQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "SmallQuantiy", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "MediumQuantiy", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "LargeQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ExtraLargeQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "WaistLength", c => c.Boolean());
            AddColumn("dbo.Products", "BurstSize", c => c.Boolean());
            AddColumn("dbo.Products", "ShoulderLength", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ShoulderLength");
            DropColumn("dbo.Products", "BurstSize");
            DropColumn("dbo.Products", "WaistLength");
            DropColumn("dbo.Products", "ExtraLargeQuantity");
            DropColumn("dbo.Products", "LargeQuantity");
            DropColumn("dbo.Products", "MediumQuantiy");
            DropColumn("dbo.Products", "SmallQuantiy");
            DropColumn("dbo.Products", "ExtraSmallQuantity");
            DropColumn("dbo.LineItems", "Description");
            DropColumn("dbo.LineItems", "UnitPrice");
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
    }
}

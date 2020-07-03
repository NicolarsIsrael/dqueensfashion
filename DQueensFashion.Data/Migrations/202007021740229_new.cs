namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailingLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "Phone", c => c.String());
            AddColumn("dbo.Orders", "Address", c => c.String());
            AddColumn("dbo.Products", "SizeChartImageId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "DeliveryDaysDuration", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Bust", c => c.Boolean());
            AddColumn("dbo.Reviews", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Reviews", "LineItemId", c => c.Int(nullable: false));
            AddColumn("dbo.WishLists", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.WishLists", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.WishLists", "CategoryName", c => c.String());
            DropColumn("dbo.Customers", "Fullname");
            DropColumn("dbo.Products", "ExtraSmallQuantity");
            DropColumn("dbo.Products", "SmallQuantiy");
            DropColumn("dbo.Products", "MediumQuantiy");
            DropColumn("dbo.Products", "LargeQuantity");
            DropColumn("dbo.Products", "ExtraLargeQuantity");
            DropColumn("dbo.Products", "Burst");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Burst", c => c.Boolean());
            AddColumn("dbo.Products", "ExtraLargeQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "LargeQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "MediumQuantiy", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "SmallQuantiy", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ExtraSmallQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "Fullname", c => c.String());
            DropColumn("dbo.WishLists", "CategoryName");
            DropColumn("dbo.WishLists", "CategoryId");
            DropColumn("dbo.WishLists", "ProductPrice");
            DropColumn("dbo.Reviews", "LineItemId");
            DropColumn("dbo.Reviews", "CustomerId");
            DropColumn("dbo.Products", "Bust");
            DropColumn("dbo.Products", "DeliveryDaysDuration");
            DropColumn("dbo.Products", "SizeChartImageId");
            DropColumn("dbo.Orders", "Address");
            DropColumn("dbo.Orders", "Phone");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "FirstName");
            DropTable("dbo.MailingLists");
        }
    }
}

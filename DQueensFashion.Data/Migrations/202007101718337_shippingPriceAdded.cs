namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shippingPriceAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SubTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "ShippingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ShippingPrice");
            DropColumn("dbo.Orders", "SubTotal");
        }
    }
}

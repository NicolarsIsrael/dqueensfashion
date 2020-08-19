namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usashippingprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralValues", "UsaShippingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneralValues", "OtherShippingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.GeneralValues", "ShippingPrice");
            DropColumn("dbo.GeneralValues", "MinFreeShippingPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneralValues", "MinFreeShippingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneralValues", "ShippingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.GeneralValues", "OtherShippingPrice");
            DropColumn("dbo.GeneralValues", "UsaShippingPrice");
        }
    }
}

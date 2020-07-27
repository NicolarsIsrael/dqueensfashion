namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class freeshipping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralValues", "MinFreeShippingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneralValues", "MinFreeShippingPrice");
        }
    }
}

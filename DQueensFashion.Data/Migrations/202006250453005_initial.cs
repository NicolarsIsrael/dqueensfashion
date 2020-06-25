namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SizeChartImageId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "DeliveryDaysDuration", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "SizeChartId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "SizeChartId", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "DeliveryDaysDuration");
            DropColumn("dbo.Products", "SizeChartImageId");
        }
    }
}

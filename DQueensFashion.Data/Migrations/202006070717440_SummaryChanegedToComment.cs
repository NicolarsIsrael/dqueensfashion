namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SummaryChanegedToComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "SubTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Reviews", "Comment", c => c.String(nullable: false));
            DropColumn("dbo.Reviews", "Summary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "Summary", c => c.String(nullable: false));
            DropColumn("dbo.Reviews", "Comment");
            DropColumn("dbo.Products", "SubTotal");
            DropColumn("dbo.Products", "Discount");
        }
    }
}

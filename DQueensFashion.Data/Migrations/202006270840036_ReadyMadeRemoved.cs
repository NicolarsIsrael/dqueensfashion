namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReadyMadeRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "ExtraSmallQuantity");
            DropColumn("dbo.Products", "SmallQuantiy");
            DropColumn("dbo.Products", "MediumQuantiy");
            DropColumn("dbo.Products", "LargeQuantity");
            DropColumn("dbo.Products", "ExtraLargeQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ExtraLargeQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "LargeQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "MediumQuantiy", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "SmallQuantiy", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ExtraSmallQuantity", c => c.Int(nullable: false));
        }
    }
}

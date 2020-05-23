namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageAddedToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String());
            AddColumn("dbo.Products", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "MainImagePath", c => c.String());
            AddColumn("dbo.Products", "AuxiliaryImagePath1", c => c.String());
            AddColumn("dbo.Products", "AuxiliaryImagePath2", c => c.String());
            AddColumn("dbo.Products", "AuxiliaryImagePath3", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AuxiliaryImagePath3");
            DropColumn("dbo.Products", "AuxiliaryImagePath2");
            DropColumn("dbo.Products", "AuxiliaryImagePath1");
            DropColumn("dbo.Products", "MainImagePath");
            DropColumn("dbo.Products", "Price");
            DropColumn("dbo.Products", "Description");
        }
    }
}

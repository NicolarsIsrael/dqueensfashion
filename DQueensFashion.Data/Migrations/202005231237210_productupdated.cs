namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ImagePath1", c => c.String());
            AddColumn("dbo.Products", "ImagePath2", c => c.String());
            AddColumn("dbo.Products", "ImagePath3", c => c.String());
            AddColumn("dbo.Products", "ImagePath4", c => c.String());
            DropColumn("dbo.Products", "MainImagePath");
            DropColumn("dbo.Products", "AuxiliaryImagePath1");
            DropColumn("dbo.Products", "AuxiliaryImagePath2");
            DropColumn("dbo.Products", "AuxiliaryImagePath3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "AuxiliaryImagePath3", c => c.String());
            AddColumn("dbo.Products", "AuxiliaryImagePath2", c => c.String());
            AddColumn("dbo.Products", "AuxiliaryImagePath1", c => c.String());
            AddColumn("dbo.Products", "MainImagePath", c => c.String());
            DropColumn("dbo.Products", "ImagePath4");
            DropColumn("dbo.Products", "ImagePath3");
            DropColumn("dbo.Products", "ImagePath2");
            DropColumn("dbo.Products", "ImagePath1");
        }
    }
}

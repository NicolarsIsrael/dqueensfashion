namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageFIleAddedToDb : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "ImagePath1");
            DropColumn("dbo.Products", "ImagePath2");
            DropColumn("dbo.Products", "ImagePath3");
            DropColumn("dbo.Products", "ImagePath4");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImagePath4", c => c.String());
            AddColumn("dbo.Products", "ImagePath3", c => c.String());
            AddColumn("dbo.Products", "ImagePath2", c => c.String());
            AddColumn("dbo.Products", "ImagePath1", c => c.String());
        }
    }
}

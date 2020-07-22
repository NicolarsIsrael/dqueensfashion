namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numberOFItemsDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "NumberOfItemsBought", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "NumberOfItemsBought");
        }
    }
}

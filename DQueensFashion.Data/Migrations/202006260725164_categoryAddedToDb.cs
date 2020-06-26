namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryAddedToDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishLists", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.WishLists", "CategoryName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WishLists", "CategoryName");
            DropColumn("dbo.WishLists", "CategoryId");
        }
    }
}

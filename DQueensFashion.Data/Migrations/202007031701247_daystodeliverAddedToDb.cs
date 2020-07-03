namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class daystodeliverAddedToDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "DaysToDeliver", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "DaysToDeliver");
        }
    }
}

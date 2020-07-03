namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class daystodeliverAddedToDb2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "DaysToDeliver");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "DaysToDeliver", c => c.Int(nullable: false));
        }
    }
}

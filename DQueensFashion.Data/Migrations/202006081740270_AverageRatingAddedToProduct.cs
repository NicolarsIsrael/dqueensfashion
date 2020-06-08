namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AverageRatingAddedToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AverageRating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AverageRating");
        }
    }
}

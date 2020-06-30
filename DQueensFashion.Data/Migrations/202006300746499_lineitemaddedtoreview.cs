namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lineitemaddedtoreview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Reviews", "LineItemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "LineItemId");
            DropColumn("dbo.Reviews", "CustomerId");
        }
    }
}

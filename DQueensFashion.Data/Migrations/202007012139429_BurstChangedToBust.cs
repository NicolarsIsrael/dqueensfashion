namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BurstChangedToBust : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Bust", c => c.Boolean());
            DropColumn("dbo.Products", "Burst");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Burst", c => c.Boolean());
            DropColumn("dbo.Products", "Bust");
        }
    }
}

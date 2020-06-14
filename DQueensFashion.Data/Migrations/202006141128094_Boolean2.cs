namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Boolean2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ShoulderLength", c => c.Boolean());
            DropColumn("dbo.Products", "ShouldLength");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ShouldLength", c => c.Boolean());
            DropColumn("dbo.Products", "ShoulderLength");
        }
    }
}

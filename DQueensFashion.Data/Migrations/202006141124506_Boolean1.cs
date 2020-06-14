namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Boolean1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "WaistLength", c => c.Boolean());
            AddColumn("dbo.Products", "ShouldLength", c => c.Boolean());
            DropColumn("dbo.Products", "WaistSize");
            DropColumn("dbo.Products", "Shoulder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Shoulder", c => c.Boolean());
            AddColumn("dbo.Products", "WaistSize", c => c.Boolean());
            DropColumn("dbo.Products", "ShouldLength");
            DropColumn("dbo.Products", "WaistLength");
        }
    }
}

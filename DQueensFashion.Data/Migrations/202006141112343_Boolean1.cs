namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Boolean1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "BurstSize", c => c.Boolean());
            AlterColumn("dbo.Products", "WaistSize", c => c.Boolean());
            AlterColumn("dbo.Products", "Shoulder", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Shoulder", c => c.String());
            AlterColumn("dbo.Products", "WaistSize", c => c.String());
            AlterColumn("dbo.Products", "BurstSize", c => c.String());
        }
    }
}

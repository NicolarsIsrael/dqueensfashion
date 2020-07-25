namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qtyvariation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "QuantityVariation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "QuantityVariation");
        }
    }
}

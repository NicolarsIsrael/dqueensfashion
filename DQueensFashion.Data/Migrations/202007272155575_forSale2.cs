namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forSale2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ForSale", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ForSale", c => c.Boolean());
        }
    }
}

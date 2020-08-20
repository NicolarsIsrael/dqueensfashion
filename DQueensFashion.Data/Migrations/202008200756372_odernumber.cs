namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class odernumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderNumber");
        }
    }
}

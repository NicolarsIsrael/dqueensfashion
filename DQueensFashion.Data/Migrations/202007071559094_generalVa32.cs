namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class generalVa32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "AvailableSubcriptionDiscount", c => c.Boolean());
            AddColumn("dbo.Customers", "UsedSubscriptionDiscount", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "UsedSubscriptionDiscount");
            DropColumn("dbo.Customers", "AvailableSubcriptionDiscount");
        }
    }
}

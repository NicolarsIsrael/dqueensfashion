namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralValues", "NewsLetterSubscriptionDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.GeneralValues", "NewsChannelSubscriptionDiscount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneralValues", "NewsChannelSubscriptionDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.GeneralValues", "NewsLetterSubscriptionDiscount");
        }
    }
}

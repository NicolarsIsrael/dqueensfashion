namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userIdChangedtoString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "UserId", c => c.Int(nullable: false));
        }
    }
}

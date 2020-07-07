namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class generalValuesRemoved1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneralValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        NewsLetterSubscriptionDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GeneralValues");
        }
    }
}

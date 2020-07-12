namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageAddedToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Subject = c.String(),
                        MessageSummary = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        DateModifiedUtc = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}

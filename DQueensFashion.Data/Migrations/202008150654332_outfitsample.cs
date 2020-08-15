namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class outfitsample : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OutfitSamples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OutfitName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        DateModifiedUtc = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OutfitSampleImageFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        OutfitSampleId = c.Int(nullable: false),
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
            DropTable("dbo.OutfitSampleImageFiles");
            DropTable("dbo.OutfitSamples");
        }
    }
}

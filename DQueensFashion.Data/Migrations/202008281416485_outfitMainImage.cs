namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class outfitMainImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralValues", "OutfitMainPicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneralValues", "OutfitMainPicture");
        }
    }
}

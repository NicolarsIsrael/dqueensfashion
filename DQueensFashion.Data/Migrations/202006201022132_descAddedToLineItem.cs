namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descAddedToLineItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "Description");
        }
    }
}

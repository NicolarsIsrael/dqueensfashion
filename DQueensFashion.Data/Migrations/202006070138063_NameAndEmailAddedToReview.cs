namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameAndEmailAddedToReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Reviews", "Email", c => c.String());
            AlterColumn("dbo.Reviews", "Summary", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Summary", c => c.String());
            DropColumn("dbo.Reviews", "Email");
            DropColumn("dbo.Reviews", "Name");
        }
    }
}

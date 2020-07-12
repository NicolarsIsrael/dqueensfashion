namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReadAddedToDb2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "Read", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "Read", c => c.Boolean());
        }
    }
}

namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class generalValueschanged : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GeneralValues", "Address");
            DropColumn("dbo.GeneralValues", "Email");
            DropColumn("dbo.GeneralValues", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneralValues", "PhoneNumber", c => c.String());
            AddColumn("dbo.GeneralValues", "Email", c => c.String());
            AddColumn("dbo.GeneralValues", "Address", c => c.String());
        }
    }
}

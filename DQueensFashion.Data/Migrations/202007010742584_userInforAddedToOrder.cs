namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userInforAddedToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "Phone", c => c.String());
            AddColumn("dbo.Orders", "Address", c => c.String());
            DropColumn("dbo.Customers", "Fullname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Fullname", c => c.String());
            DropColumn("dbo.Orders", "Address");
            DropColumn("dbo.Orders", "Phone");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "FirstName");
        }
    }
}

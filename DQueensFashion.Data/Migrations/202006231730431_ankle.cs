namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ankle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "RoundAnkle", c => c.Boolean());
            DropColumn("dbo.Products", "RoundNeck");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "RoundNeck", c => c.Boolean());
            DropColumn("dbo.Products", "RoundAnkle");
        }
    }
}

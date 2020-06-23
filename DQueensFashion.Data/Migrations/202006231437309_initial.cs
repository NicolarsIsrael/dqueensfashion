namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Shoulder", c => c.Boolean());
            AddColumn("dbo.Products", "ArmHole", c => c.Boolean());
            AddColumn("dbo.Products", "Burst", c => c.Boolean());
            AddColumn("dbo.Products", "Waist", c => c.Boolean());
            AddColumn("dbo.Products", "Hips", c => c.Boolean());
            AddColumn("dbo.Products", "Thigh", c => c.Boolean());
            AddColumn("dbo.Products", "FullBodyLength", c => c.Boolean());
            AddColumn("dbo.Products", "KneeGarmentLength", c => c.Boolean());
            AddColumn("dbo.Products", "TopLength", c => c.Boolean());
            AddColumn("dbo.Products", "TrousersLength", c => c.Boolean());
            AddColumn("dbo.Products", "RoundNeck", c => c.Boolean());
            AddColumn("dbo.Products", "NipNip", c => c.Boolean());
            AddColumn("dbo.Products", "SleeveLength", c => c.Boolean());
            DropColumn("dbo.Products", "WaistLength");
            DropColumn("dbo.Products", "BurstSize");
            DropColumn("dbo.Products", "ShoulderLength");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ShoulderLength", c => c.Boolean());
            AddColumn("dbo.Products", "BurstSize", c => c.Boolean());
            AddColumn("dbo.Products", "WaistLength", c => c.Boolean());
            DropColumn("dbo.Products", "SleeveLength");
            DropColumn("dbo.Products", "NipNip");
            DropColumn("dbo.Products", "RoundNeck");
            DropColumn("dbo.Products", "TrousersLength");
            DropColumn("dbo.Products", "TopLength");
            DropColumn("dbo.Products", "KneeGarmentLength");
            DropColumn("dbo.Products", "FullBodyLength");
            DropColumn("dbo.Products", "Thigh");
            DropColumn("dbo.Products", "Hips");
            DropColumn("dbo.Products", "Waist");
            DropColumn("dbo.Products", "Burst");
            DropColumn("dbo.Products", "ArmHole");
            DropColumn("dbo.Products", "Shoulder");
        }
    }
}

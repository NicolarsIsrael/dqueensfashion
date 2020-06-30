namespace DQueensFashion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priceAddedToWishlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishLists", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WishLists", "ProductPrice");
        }
    }
}

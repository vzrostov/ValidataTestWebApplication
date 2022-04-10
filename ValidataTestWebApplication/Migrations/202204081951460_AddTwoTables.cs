namespace ValidataTestWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTwoTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Order_OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Orders", t => t.Order_OrderId)
                .Index(t => t.Order_OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.Items", new[] { "Order_OrderId" });
            DropTable("dbo.Items");
        }
    }
}

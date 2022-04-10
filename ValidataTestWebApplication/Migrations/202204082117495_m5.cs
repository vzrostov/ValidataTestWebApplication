namespace ValidataTestWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m5 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Items", name: "Order_OrderId", newName: "OrderID");
            RenameIndex(table: "dbo.Items", name: "IX_Order_OrderId", newName: "IX_OrderID");
            AddColumn("dbo.Items", "ProductID", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "ProductID");
            AddForeignKey("dbo.Items", "ProductID", "dbo.Products", "ProductId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "ProductID", "dbo.Products");
            DropIndex("dbo.Items", new[] { "ProductID" });
            DropColumn("dbo.Items", "ProductID");
            RenameIndex(table: "dbo.Items", name: "IX_OrderID", newName: "IX_Order_OrderId");
            RenameColumn(table: "dbo.Items", name: "OrderID", newName: "Order_OrderId");
        }
    }
}

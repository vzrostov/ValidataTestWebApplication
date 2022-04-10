namespace ValidataTestWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Customer_CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "Customer_CustomerID" });
            DropIndex("dbo.Items", new[] { "Order_OrderId" });
            AlterColumn("dbo.Customers", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Customers", "LastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Customers", "PostalCode", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Orders", "Customer_CustomerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Items", "Order_OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "Customer_CustomerID");
            CreateIndex("dbo.Items", "Order_OrderId");
            AddForeignKey("dbo.Orders", "Customer_CustomerID", "dbo.Customers", "CustomerID", cascadeDelete: true);
            AddForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Customer_CustomerID", "dbo.Customers");
            DropIndex("dbo.Items", new[] { "Order_OrderId" });
            DropIndex("dbo.Orders", new[] { "Customer_CustomerID" });
            AlterColumn("dbo.Items", "Order_OrderId", c => c.Int());
            AlterColumn("dbo.Orders", "Customer_CustomerID", c => c.Int());
            AlterColumn("dbo.Customers", "PostalCode", c => c.String());
            AlterColumn("dbo.Customers", "Address", c => c.String());
            AlterColumn("dbo.Customers", "LastName", c => c.String());
            AlterColumn("dbo.Customers", "FirstName", c => c.String());
            CreateIndex("dbo.Items", "Order_OrderId");
            CreateIndex("dbo.Orders", "Customer_CustomerID");
            AddForeignKey("dbo.Items", "Order_OrderId", "dbo.Orders", "OrderId");
            AddForeignKey("dbo.Orders", "Customer_CustomerID", "dbo.Customers", "CustomerID");
        }
    }
}

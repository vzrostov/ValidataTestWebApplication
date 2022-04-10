namespace ValidataTestWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "Customer_CustomerID", newName: "CustomerID");
            RenameIndex(table: "dbo.Orders", name: "IX_Customer_CustomerID", newName: "IX_CustomerID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Orders", name: "IX_CustomerID", newName: "IX_Customer_CustomerID");
            RenameColumn(table: "dbo.Orders", name: "CustomerID", newName: "Customer_CustomerID");
        }
    }
}

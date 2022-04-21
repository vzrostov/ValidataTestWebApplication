namespace ValidataTestWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m80 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Price", c => c.Single(nullable: false));
        }
    }
}

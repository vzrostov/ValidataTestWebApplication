namespace ValidataTestWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m61 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Quantity", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Quantity");
        }
    }
}

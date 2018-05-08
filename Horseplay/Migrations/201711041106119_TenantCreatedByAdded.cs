namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenantCreatedByAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "createdBy", c => c.Int(nullable: false));
            DropColumn("dbo.Tenants", "userId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tenants", "userId", c => c.Int(nullable: false));
            DropColumn("dbo.Tenants", "createdBy");
        }
    }
}

namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieTenantId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyGroups", "TenantId", c => c.Int(nullable: false));
            AddColumn("dbo.Contacts", "TenantId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "TenantId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TenantId");
            DropColumn("dbo.Contacts", "TenantId");
            DropColumn("dbo.CompanyGroups", "TenantId");
        }
    }
}

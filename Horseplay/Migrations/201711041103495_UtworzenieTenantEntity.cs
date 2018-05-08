namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UtworzenieTenantEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        TenantId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Code = c.String(),
                        City = c.String(),
                        createdOn = c.DateTime(),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tenants");
        }
    }
}

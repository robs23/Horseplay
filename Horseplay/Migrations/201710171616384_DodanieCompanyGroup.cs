namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieCompanyGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyGroups",
                c => new
                    {
                        CompanyGroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CompanyGroupId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompanyGroups");
        }
    }
}

namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeesWithRelationships : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeGroups",
                c => new
                    {
                        EmployeeGroupId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        dateAdded = c.DateTime(),
                        addedBy = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeGroupId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        dateAdded = c.DateTime(),
                        addedBy = c.Int(),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        EmployeeGroupId = c.Int(),
                        BirthDay = c.DateTime(),
                        EmployedOn = c.DateTime(),
                        Phone = c.String(),
                        Mobile = c.String(),
                        Mail = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.EmployeesAndGroups",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false),
                        EmployeeGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeId, t.EmployeeGroupId })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeeGroups", t => t.EmployeeGroupId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.EmployeeGroupId);
            
            CreateIndex("dbo.Users", "TenantId");
            AddForeignKey("dbo.Users", "TenantId", "dbo.Tenants", "TenantId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.EmployeesAndGroups", "EmployeeGroupId", "dbo.EmployeeGroups");
            DropForeignKey("dbo.EmployeesAndGroups", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmployeesAndGroups", new[] { "EmployeeGroupId" });
            DropIndex("dbo.EmployeesAndGroups", new[] { "EmployeeId" });
            DropIndex("dbo.Users", new[] { "TenantId" });
            DropTable("dbo.EmployeesAndGroups");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeGroups");
        }
    }
}

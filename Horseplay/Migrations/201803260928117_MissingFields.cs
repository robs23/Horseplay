namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MissingFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeGroups", "dateModified", c => c.DateTime());
            AddColumn("dbo.EmployeeGroups", "modifiedBy", c => c.Int());
            AddColumn("dbo.Employees", "dateModified", c => c.DateTime());
            AddColumn("dbo.Employees", "modifiedBy", c => c.Int());
            AddColumn("dbo.Employees", "PersonalIdentityNumber", c => c.String());
            AddColumn("dbo.Employees", "Country", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "IdNumber", c => c.String());
            AddColumn("dbo.Employees", "ContractType", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ExpirationDate", c => c.DateTime());
            AddColumn("dbo.Vehicles", "Brand", c => c.String());
            AddColumn("dbo.Vehicles", "Model", c => c.String());
            AddColumn("dbo.Vehicles", "ProductionYear", c => c.Int());
            AddColumn("dbo.Vehicles", "Vin", c => c.String());
            AddColumn("dbo.Vehicles", "Capacity", c => c.String());
            AddColumn("dbo.Vehicles", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "RegistrationDate", c => c.DateTime());
            AddColumn("dbo.Vehicles", "ServiceDate", c => c.DateTime());
            AddColumn("dbo.Vehicles", "DefaultUser_EmployeeId", c => c.Int());
            CreateIndex("dbo.Vehicles", "DefaultUser_EmployeeId");
            AddForeignKey("dbo.Vehicles", "DefaultUser_EmployeeId", "dbo.Employees", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "DefaultUser_EmployeeId", "dbo.Employees");
            DropIndex("dbo.Vehicles", new[] { "DefaultUser_EmployeeId" });
            DropColumn("dbo.Vehicles", "DefaultUser_EmployeeId");
            DropColumn("dbo.Vehicles", "ServiceDate");
            DropColumn("dbo.Vehicles", "RegistrationDate");
            DropColumn("dbo.Vehicles", "Type");
            DropColumn("dbo.Vehicles", "Capacity");
            DropColumn("dbo.Vehicles", "Vin");
            DropColumn("dbo.Vehicles", "ProductionYear");
            DropColumn("dbo.Vehicles", "Model");
            DropColumn("dbo.Vehicles", "Brand");
            DropColumn("dbo.Employees", "ExpirationDate");
            DropColumn("dbo.Employees", "ContractType");
            DropColumn("dbo.Employees", "IdNumber");
            DropColumn("dbo.Employees", "Country");
            DropColumn("dbo.Employees", "PersonalIdentityNumber");
            DropColumn("dbo.Employees", "modifiedBy");
            DropColumn("dbo.Employees", "dateModified");
            DropColumn("dbo.EmployeeGroups", "modifiedBy");
            DropColumn("dbo.EmployeeGroups", "dateModified");
        }
    }
}

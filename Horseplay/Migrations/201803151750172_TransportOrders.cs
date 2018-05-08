namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        DateModified = c.DateTime(),
                        ModifiedBy = c.Int(),
                        Name = c.String(nullable: false),
                        Street = c.String(),
                        ZipCode = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        DateModified = c.DateTime(),
                        ModifiedBy = c.Int(),
                        AddressId = c.Int(nullable: false),
                        TaxRegisterNumber = c.String(),
                        BusinessRegisterNumber = c.String(),
                        CourtRegisterNumber = c.String(),
                        PaymentTerm = c.Int(),
                        Type = c.Int(nullable: false),
                        ContactDetail_ContactDetailId = c.Int(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.ContactDetails", t => t.ContactDetail_ContactDetailId)
                .Index(t => t.AddressId)
                .Index(t => t.ContactDetail_ContactDetailId);
            
            CreateTable(
                "dbo.ContactDetails",
                c => new
                    {
                        ContactDetailId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        DateModified = c.DateTime(),
                        ModifiedBy = c.Int(),
                        Phone = c.String(),
                        Mobile = c.String(),
                        Fax = c.String(),
                        Mail = c.String(),
                        PrivateMail = c.String(),
                        PrivatePhone = c.String(),
                    })
                .PrimaryKey(t => t.ContactDetailId);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        RouteId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        DateModified = c.DateTime(),
                        ModifiedBy = c.Int(),
                        Length = c.Int(),
                    })
                .PrimaryKey(t => t.RouteId);
            
            CreateTable(
                "dbo.RouteStops",
                c => new
                    {
                        RouteStopId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        DateModified = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ArrivalDate = c.DateTime(),
                        DepartureDate = c.DateTime(),
                        StopType = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        AddressId = c.Int(),
                        RouteId = c.Int(),
                    })
                .PrimaryKey(t => t.RouteStopId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Routes", t => t.RouteId)
                .Index(t => t.AddressId)
                .Index(t => t.RouteId);
            
            CreateTable(
                "dbo.TransportOrders",
                c => new
                    {
                        TransportOrderId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        DateModified = c.DateTime(),
                        ModifiedBy = c.Int(),
                        Cost = c.Single(),
                        Remarks = c.String(),
                        Type = c.Int(nullable: false),
                        RouteId = c.Int(),
                        Carrier_CompanyId = c.Int(),
                        Customer_CompanyId = c.Int(),
                        PrimaryDriver_EmployeeId = c.Int(),
                        SecondaryDriver_EmployeeId = c.Int(),
                        Trailer_VehicleId = c.Int(),
                        Truck_VehicleId = c.Int(),
                    })
                .PrimaryKey(t => t.TransportOrderId)
                .ForeignKey("dbo.Companies", t => t.Carrier_CompanyId)
                .ForeignKey("dbo.Companies", t => t.Customer_CompanyId)
                .ForeignKey("dbo.Employees", t => t.PrimaryDriver_EmployeeId)
                .ForeignKey("dbo.Routes", t => t.RouteId)
                .ForeignKey("dbo.Employees", t => t.SecondaryDriver_EmployeeId)
                .ForeignKey("dbo.Vehicles", t => t.Trailer_VehicleId)
                .ForeignKey("dbo.Vehicles", t => t.Truck_VehicleId)
                .Index(t => t.RouteId)
                .Index(t => t.Carrier_CompanyId)
                .Index(t => t.Customer_CompanyId)
                .Index(t => t.PrimaryDriver_EmployeeId)
                .Index(t => t.SecondaryDriver_EmployeeId)
                .Index(t => t.Trailer_VehicleId)
                .Index(t => t.Truck_VehicleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportOrders", "Truck_VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.TransportOrders", "Trailer_VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.TransportOrders", "SecondaryDriver_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.TransportOrders", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.TransportOrders", "PrimaryDriver_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.TransportOrders", "Customer_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.TransportOrders", "Carrier_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.RouteStops", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.RouteStops", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Companies", "ContactDetail_ContactDetailId", "dbo.ContactDetails");
            DropForeignKey("dbo.Companies", "AddressId", "dbo.Addresses");
            DropIndex("dbo.TransportOrders", new[] { "Truck_VehicleId" });
            DropIndex("dbo.TransportOrders", new[] { "Trailer_VehicleId" });
            DropIndex("dbo.TransportOrders", new[] { "SecondaryDriver_EmployeeId" });
            DropIndex("dbo.TransportOrders", new[] { "PrimaryDriver_EmployeeId" });
            DropIndex("dbo.TransportOrders", new[] { "Customer_CompanyId" });
            DropIndex("dbo.TransportOrders", new[] { "Carrier_CompanyId" });
            DropIndex("dbo.TransportOrders", new[] { "RouteId" });
            DropIndex("dbo.RouteStops", new[] { "RouteId" });
            DropIndex("dbo.RouteStops", new[] { "AddressId" });
            DropIndex("dbo.Companies", new[] { "ContactDetail_ContactDetailId" });
            DropIndex("dbo.Companies", new[] { "AddressId" });
            DropTable("dbo.TransportOrders");
            DropTable("dbo.RouteStops");
            DropTable("dbo.Routes");
            DropTable("dbo.ContactDetails");
            DropTable("dbo.Companies");
            DropTable("dbo.Addresses");
        }
    }
}

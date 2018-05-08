namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vehicles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        dateAdded = c.DateTime(),
                        addedBy = c.Int(),
                        dateModified = c.DateTime(),
                        modifiedBy = c.Int(),
                        Plate = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.VehicleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}

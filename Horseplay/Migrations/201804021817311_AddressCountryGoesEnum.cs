namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressCountryGoesEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "Country", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "Country", c => c.String());
        }
    }
}

namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieCreatedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "createdOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "lastLogged", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "lastLogged");
            DropColumn("dbo.Users", "createdOn");
        }
    }
}

namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCreatedOnToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "createdOn", c => c.DateTime());
            AlterColumn("dbo.Users", "lastLogged", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "lastLogged", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "createdOn", c => c.DateTime(nullable: false));
        }
    }
}

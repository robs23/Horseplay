namespace Horseplay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvitationWithUserConfirmation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        InvitationId = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        AddedBy = c.Int(),
                        InvitationToken = c.String(),
                        InvitedMail = c.String(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        AcceptedOn = c.DateTime(),
                        IsAccepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InvitationId);
            
            AddColumn("dbo.Users", "ConfirmationToken", c => c.String());
            AddColumn("dbo.Users", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ConfirmationDate", c => c.DateTime());
            AddColumn("dbo.Users", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ExpirationDate");
            DropColumn("dbo.Users", "ConfirmationDate");
            DropColumn("dbo.Users", "IsConfirmed");
            DropColumn("dbo.Users", "ConfirmationToken");
            DropTable("dbo.Invitations");
        }
    }
}

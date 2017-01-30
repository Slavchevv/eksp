namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtmupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkRolesUsersDetails", "UserDetailsId", "dbo.UserDetails");
            DropIndex("dbo.WorkRolesUsersDetails", new[] { "UserDetailsId" });
            DropPrimaryKey("dbo.WorkRolesUsersDetails");
            AddColumn("dbo.WorkRolesUsersDetails", "UserDetails_UserDetailsId", c => c.Int());
            AlterColumn("dbo.WorkRolesUsersDetails", "UserDetailsId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.WorkRolesUsersDetails", new[] { "WorkRoleId", "UserDetailsId" });
            CreateIndex("dbo.WorkRolesUsersDetails", "UserDetails_UserDetailsId");
            AddForeignKey("dbo.WorkRolesUsersDetails", "UserDetails_UserDetailsId", "dbo.UserDetails", "UserDetailsId");
            DropColumn("dbo.WorkRolesUsersDetails", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkRolesUsersDetails", "IsActive", c => c.Boolean());
            DropForeignKey("dbo.WorkRolesUsersDetails", "UserDetails_UserDetailsId", "dbo.UserDetails");
            DropIndex("dbo.WorkRolesUsersDetails", new[] { "UserDetails_UserDetailsId" });
            DropPrimaryKey("dbo.WorkRolesUsersDetails");
            AlterColumn("dbo.WorkRolesUsersDetails", "UserDetailsId", c => c.Int(nullable: false));
            DropColumn("dbo.WorkRolesUsersDetails", "UserDetails_UserDetailsId");
            AddPrimaryKey("dbo.WorkRolesUsersDetails", new[] { "WorkRoleId", "UserDetailsId" });
            CreateIndex("dbo.WorkRolesUsersDetails", "UserDetailsId");
            AddForeignKey("dbo.WorkRolesUsersDetails", "UserDetailsId", "dbo.UserDetails", "UserDetailsId", cascadeDelete: true);
        }
    }
}

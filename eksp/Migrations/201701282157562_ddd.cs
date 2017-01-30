namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ddd : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.WorkRolesUsersDetails");
            AddColumn("dbo.WorkRolesUsersDetails", "WRUDId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.WorkRolesUsersDetails", new[] { "WRUDId", "WorkRoleId", "UserDetailsId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.WorkRolesUsersDetails");
            DropColumn("dbo.WorkRolesUsersDetails", "WRUDId");
            AddPrimaryKey("dbo.WorkRolesUsersDetails", new[] { "WorkRoleId", "UserDetailsId" });
        }
    }
}

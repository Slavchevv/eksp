namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytomanywrusers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkRolesUsersDetails",
                c => new
                    {
                        WorkRoleId = c.Int(nullable: false),
                        UserDetailsId = c.Int(nullable: false),
                        FocusStart = c.DateTime(nullable: false),
                        FocusEnd = c.DateTime(nullable: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.WorkRoleId, t.UserDetailsId })
                .ForeignKey("dbo.UserDetails", t => t.UserDetailsId, cascadeDelete: false)//changed to false
                .ForeignKey("dbo.WorkRoles", t => t.WorkRoleId, cascadeDelete: false)//changed to false
                .Index(t => t.WorkRoleId)
                .Index(t => t.UserDetailsId);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkRolesUsersDetails", "WorkRoleId", "dbo.WorkRoles");
            DropForeignKey("dbo.WorkRolesUsersDetails", "UserDetailsId", "dbo.UserDetails");
            DropIndex("dbo.WorkRolesUsersDetails", new[] { "UserDetailsId" });
            DropIndex("dbo.WorkRolesUsersDetails", new[] { "WorkRoleId" });
            DropTable("dbo.WorkRolesUsersDetails");
        }
    }
}

namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workrole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkRoles",
                c => new
                    {
                        WorkRoleId = c.Int(nullable: false, identity: true),
                        RoleDescription = c.String(),
                        Company_CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.WorkRoleId)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .Index(t => t.Company_CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkRoles", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.WorkRoles", new[] { "Company_CompanyId" });
            DropTable("dbo.WorkRoles");
        }
    }
}

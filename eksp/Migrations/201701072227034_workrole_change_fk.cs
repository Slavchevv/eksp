namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workrole_change_fk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkRoles", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.WorkRoles", new[] { "Company_CompanyId" });
            RenameColumn(table: "dbo.WorkRoles", name: "Company_CompanyId", newName: "CompanyId");
            AlterColumn("dbo.WorkRoles", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkRoles", "CompanyId");
            AddForeignKey("dbo.WorkRoles", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkRoles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.WorkRoles", new[] { "CompanyId" });
            AlterColumn("dbo.WorkRoles", "CompanyId", c => c.Int());
            RenameColumn(table: "dbo.WorkRoles", name: "CompanyId", newName: "Company_CompanyId");
            CreateIndex("dbo.WorkRoles", "Company_CompanyId");
            AddForeignKey("dbo.WorkRoles", "Company_CompanyId", "dbo.Companies", "CompanyId");
        }
    }
}

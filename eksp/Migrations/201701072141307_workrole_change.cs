namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workrole_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkRoles", "RoleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkRoles", "RoleName");
        }
    }
}

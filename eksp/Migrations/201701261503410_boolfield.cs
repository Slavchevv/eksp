namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boolfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkRolesUsersDetails", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkRolesUsersDetails", "isActive");
        }
    }
}

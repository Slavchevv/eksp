namespace eksp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserDetailsId = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserAddress = c.String(),
                        UserCountry = c.String(),
                        UserPostalCode = c.String(),
                        UserPhoneNumber = c.String(),
                        CompanyId = c.Int(nullable: false),
                        identtyUserId = c.String(),
                    })
                .PrimaryKey(t => t.UserDetailsId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "CompanyId", "dbo.Companies");
            DropIndex("dbo.UserDetails", new[] { "CompanyId" });
            DropTable("dbo.UserDetails");
        }
    }
}

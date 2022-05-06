namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRolesMappingTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRolesMappings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRolesMappings", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRolesMappings", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserRolesMappings", new[] { "RoleId" });
            DropIndex("dbo.UserRolesMappings", new[] { "UserId" });
            DropTable("dbo.UserRolesMappings");
        }
    }
}

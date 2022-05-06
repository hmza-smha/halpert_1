namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorksOnTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorksOns",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProjectId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorksOns", "UserId", "dbo.Users");
            DropForeignKey("dbo.WorksOns", "ProjectId", "dbo.Projects");
            DropIndex("dbo.WorksOns", new[] { "ProjectId" });
            DropIndex("dbo.WorksOns", new[] { "UserId" });
            DropTable("dbo.WorksOns");
        }
    }
}

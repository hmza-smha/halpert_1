namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Details = c.String(),
                        ProjectId = c.Int(nullable: false),
                        DevId = c.Int(),
                        Optimistic = c.Int(),
                        MostLikely = c.Int(),
                        Pessimistic = c.Int(),
                        Duration = c.Int(nullable: false, defaultValue: 1),
                        StartTime = c.DateTime(),
                        Progress = c.Int(nullable: false, defaultValue: 0),
                        Accepted = c.Boolean(nullable: false, defaultValue: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.DevId)
                .Index(t => t.ProjectId)
                .Index(t => t.DevId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "DevId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Tasks", new[] { "DevId" });
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropTable("dbo.Tasks");
        }
    }
}

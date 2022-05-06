namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPredecessorTaskTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PredecessorTasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false),
                        PreTaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskId, t.PreTaskId })
                .ForeignKey("dbo.Tasks", t => t.PreTaskId, cascadeDelete: false)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: false)
                .Index(t => t.TaskId)
                .Index(t => t.PreTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PredecessorTasks", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.PredecessorTasks", "PreTaskId", "dbo.Tasks");
            DropIndex("dbo.PredecessorTasks", new[] { "PreTaskId" });
            DropIndex("dbo.PredecessorTasks", new[] { "TaskId" });
            DropTable("dbo.PredecessorTasks");
        }
    }
}

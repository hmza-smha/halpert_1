namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectsTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "OwnerId", "dbo.Users");
            DropIndex("dbo.Projects", new[] { "OwnerId" });
            DropTable("dbo.Projects");
        }
    }
}

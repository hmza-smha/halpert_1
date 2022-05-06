namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAceptedTask : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "Accepted", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "Accepted", c => c.Boolean(nullable: false));
        }
    }
}

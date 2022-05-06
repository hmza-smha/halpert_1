namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpsToTasksTBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Operators", c => c.String());
            AddColumn("dbo.Tasks", "Operands", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Operands");
            DropColumn("dbo.Tasks", "Operators");
        }
    }
}

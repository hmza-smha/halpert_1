namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceCodeToTasksTBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "SorceCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "SorceCode");
        }
    }
}

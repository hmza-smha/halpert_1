namespace halpert.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHalsteadResultsToTasksTBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Length", c => c.Int());
            AddColumn("dbo.Tasks", "Vocabulary", c => c.Int());
            AddColumn("dbo.Tasks", "Volume", c => c.Int());
            AddColumn("dbo.Tasks", "InsideTheBoundaries", c => c.Boolean());
            AddColumn("dbo.Tasks", "Difficulty", c => c.Int());
            AddColumn("dbo.Tasks", "Effort", c => c.Int());
            AddColumn("dbo.Tasks", "TimeToUnderStand", c => c.Int());
            AddColumn("dbo.Tasks", "Bugs", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Bugs");
            DropColumn("dbo.Tasks", "TimeToUnderStand");
            DropColumn("dbo.Tasks", "Effort");
            DropColumn("dbo.Tasks", "Difficulty");
            DropColumn("dbo.Tasks", "InsideTheBoundaries");
            DropColumn("dbo.Tasks", "Volume");
            DropColumn("dbo.Tasks", "Vocabulary");
            DropColumn("dbo.Tasks", "Length");
        }
    }
}

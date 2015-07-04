namespace Rps.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CurrentPlayerID", c => c.String());
            AddColumn("dbo.Games", "WinnerID", c => c.String());
            AddColumn("dbo.Games", "GameResultID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "GameResultID");
            DropColumn("dbo.Games", "WinnerID");
            DropColumn("dbo.Games", "CurrentPlayerID");
        }
    }
}

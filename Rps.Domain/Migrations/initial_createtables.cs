namespace Rps.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Player1ID = c.String(),
                        Player1Name = c.String(),
                        Player2ID = c.String(),
                        Player2Name = c.String(),
                        SinglePlayerMode = c.Boolean(nullable: false),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        UpdatedAtUtc = c.DateTime(nullable: false),
                        NumRows = c.Int(nullable: false),
                        NumCols = c.Int(nullable: false),
                        RowsPerPlayer = c.Int(nullable: false),
                        BombsPerPlayer = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        GameID = c.Long(nullable: false),
                        PlayerID = c.String(),
                        TokenType = c.Int(nullable: false),
                        Row = c.Int(nullable: false),
                        Col = c.Int(nullable: false),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        UpdatedAtUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Games", t => t.GameID, cascadeDelete: true)
                .Index(t => t.GameID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "GameID", "dbo.Games");
            DropIndex("dbo.Tokens", new[] { "GameID" });
            DropTable("dbo.Tokens");
            DropTable("dbo.Games");
        }
    }
}

namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Game_Models : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGameScores", "GameId", "dbo.GameModels");
            DropForeignKey("dbo.UserGameScores", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGameDailyActivities", "GameId", "dbo.GameModels");
            DropForeignKey("dbo.UserGameDailyActivities", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserGameScores", new[] { "GameId" });
            DropIndex("dbo.UserGameScores", new[] { "UserId" });
            DropIndex("dbo.UserGameDailyActivities", new[] { "UserId" });
            DropIndex("dbo.UserGameDailyActivities", new[] { "GameId" });
            CreateTable(
                "dbo.DayActiveGames",
                c => new
                    {
                        Day = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        GameKey = c.String(),
                    })
                .PrimaryKey(t => t.Day);
            
            CreateTable(
                "dbo.Game1_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game2_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Score = c.Double(nullable: false),
                        Stages = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game3_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Completed = c.Boolean(nullable: false),
                        Stages = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game4_1_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Stages = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game4_2_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Stages = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game4_3_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Stages = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game5_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        QuestionsAnswered = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Game6_UserScore",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserDailyActivities",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TimePlayed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.Date })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.GameModels", "GameKey", c => c.String());
            AddColumn("dbo.GameModels", "MetadataJSON", c => c.String());
            DropColumn("dbo.GameModels", "Multiplier");
            DropColumn("dbo.GameModels", "Page");
            DropTable("dbo.UserGameScores");
            DropTable("dbo.UserGameDailyActivities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGameDailyActivities",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GameId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => new { t.UserId, t.GameId });
            
            CreateTable(
                "dbo.UserGameScores",
                c => new
                    {
                        GameId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameId, t.UserId });
            
            AddColumn("dbo.GameModels", "Page", c => c.String());
            AddColumn("dbo.GameModels", "Multiplier", c => c.Double(nullable: false));
            DropForeignKey("dbo.UserDailyActivities", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserDailyActivities", new[] { "UserId" });
            DropColumn("dbo.GameModels", "MetadataJSON");
            DropColumn("dbo.GameModels", "GameKey");
            DropTable("dbo.UserDailyActivities");
            DropTable("dbo.Game6_UserScore");
            DropTable("dbo.Game5_UserScore");
            DropTable("dbo.Game4_3_UserScore");
            DropTable("dbo.Game4_2_UserScore");
            DropTable("dbo.Game4_1_UserScore");
            DropTable("dbo.Game3_UserScore");
            DropTable("dbo.Game2_UserScore");
            DropTable("dbo.Game1_UserScore");
            DropTable("dbo.DayActiveGames");
            CreateIndex("dbo.UserGameDailyActivities", "GameId");
            CreateIndex("dbo.UserGameDailyActivities", "UserId");
            CreateIndex("dbo.UserGameScores", "UserId");
            CreateIndex("dbo.UserGameScores", "GameId");
            AddForeignKey("dbo.UserGameDailyActivities", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGameDailyActivities", "GameId", "dbo.GameModels", "GameId", cascadeDelete: true);
            AddForeignKey("dbo.UserGameScores", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGameScores", "GameId", "dbo.GameModels", "GameId", cascadeDelete: true);
        }
    }
}

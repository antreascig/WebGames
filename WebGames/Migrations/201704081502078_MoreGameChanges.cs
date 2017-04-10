namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreGameChanges : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DayActiveGames");
            AddColumn("dbo.Game1_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game2_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game3_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_1_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_2_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_3_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game5_UserScore", "Tokens", c => c.Int(nullable: false));
            AddColumn("dbo.Game6_UserScore", "Tokens", c => c.Int(nullable: false));
            AlterColumn("dbo.DayActiveGames", "Day", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DayActiveGames", "Day");
            CreateIndex("dbo.Game1_UserScore", "UserId");
            CreateIndex("dbo.Game2_UserScore", "UserId");
            CreateIndex("dbo.Game3_UserScore", "UserId");
            CreateIndex("dbo.Game4_1_UserScore", "UserId");
            CreateIndex("dbo.Game4_2_UserScore", "UserId");
            CreateIndex("dbo.Game4_3_UserScore", "UserId");
            CreateIndex("dbo.Game5_UserScore", "UserId");
            CreateIndex("dbo.Game6_UserScore", "UserId");
            AddForeignKey("dbo.Game1_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game2_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game3_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game4_1_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game4_2_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game4_3_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game5_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Game6_UserScore", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Game1_UserScore", "Score");
            DropColumn("dbo.Game2_UserScore", "Stages");
            DropColumn("dbo.Game2_UserScore", "StagesCount");
            DropColumn("dbo.Game3_UserScore", "Completed");
            DropColumn("dbo.Game3_UserScore", "Attempts");
            DropColumn("dbo.Game4_1_UserScore", "Stages");
            DropColumn("dbo.Game4_1_UserScore", "StagesCount");
            DropColumn("dbo.Game4_2_UserScore", "Stages");
            DropColumn("dbo.Game4_2_UserScore", "StagesCount");
            DropColumn("dbo.Game4_3_UserScore", "Stages");
            DropColumn("dbo.Game4_3_UserScore", "StagesCount");
            DropColumn("dbo.Game5_UserScore", "Answers");
            DropColumn("dbo.Game5_UserScore", "CorrectCount");
            DropColumn("dbo.Game5_UserScore", "IncorrectCount");
            DropColumn("dbo.Game6_UserScore", "Score");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game6_UserScore", "Score", c => c.Double(nullable: false));
            AddColumn("dbo.Game5_UserScore", "IncorrectCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game5_UserScore", "CorrectCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game5_UserScore", "Answers", c => c.String());
            AddColumn("dbo.Game4_3_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_3_UserScore", "Stages", c => c.String());
            AddColumn("dbo.Game4_2_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_2_UserScore", "Stages", c => c.String());
            AddColumn("dbo.Game4_1_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_1_UserScore", "Stages", c => c.String());
            AddColumn("dbo.Game3_UserScore", "Attempts", c => c.Int(nullable: false));
            AddColumn("dbo.Game3_UserScore", "Completed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Game2_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game2_UserScore", "Stages", c => c.String());
            AddColumn("dbo.Game1_UserScore", "Score", c => c.Double(nullable: false));
            DropForeignKey("dbo.Game6_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game5_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game4_3_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game4_2_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game4_1_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game3_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game2_UserScore", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game1_UserScore", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Game6_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game5_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game4_3_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game4_2_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game4_1_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game3_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game2_UserScore", new[] { "UserId" });
            DropIndex("dbo.Game1_UserScore", new[] { "UserId" });
            DropPrimaryKey("dbo.DayActiveGames");
            AlterColumn("dbo.DayActiveGames", "Day", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Game6_UserScore", "Tokens");
            DropColumn("dbo.Game5_UserScore", "Tokens");
            DropColumn("dbo.Game4_3_UserScore", "Tokens");
            DropColumn("dbo.Game4_2_UserScore", "Tokens");
            DropColumn("dbo.Game4_1_UserScore", "Tokens");
            DropColumn("dbo.Game3_UserScore", "Tokens");
            DropColumn("dbo.Game2_UserScore", "Tokens");
            DropColumn("dbo.Game1_UserScore", "Tokens");
            AddPrimaryKey("dbo.DayActiveGames", "Day");
        }
    }
}

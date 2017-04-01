namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class More_Game_Model_Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game2_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game3_UserScore", "Attempts", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_1_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_2_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_3_UserScore", "StagesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game5_UserScore", "Answers", c => c.String());
            AddColumn("dbo.Game5_UserScore", "CorrectCount", c => c.Int(nullable: false));
            AddColumn("dbo.Game5_UserScore", "IncorrectCount", c => c.Int(nullable: false));
            DropColumn("dbo.Game2_UserScore", "Score");
            DropColumn("dbo.Game3_UserScore", "Stages");
            DropColumn("dbo.Game5_UserScore", "QuestionsAnswered");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game5_UserScore", "QuestionsAnswered", c => c.String());
            AddColumn("dbo.Game3_UserScore", "Stages", c => c.String());
            AddColumn("dbo.Game2_UserScore", "Score", c => c.Double(nullable: false));
            DropColumn("dbo.Game5_UserScore", "IncorrectCount");
            DropColumn("dbo.Game5_UserScore", "CorrectCount");
            DropColumn("dbo.Game5_UserScore", "Answers");
            DropColumn("dbo.Game4_3_UserScore", "StagesCount");
            DropColumn("dbo.Game4_2_UserScore", "StagesCount");
            DropColumn("dbo.Game4_1_UserScore", "StagesCount");
            DropColumn("dbo.Game3_UserScore", "Attempts");
            DropColumn("dbo.Game2_UserScore", "StagesCount");
        }
    }
}

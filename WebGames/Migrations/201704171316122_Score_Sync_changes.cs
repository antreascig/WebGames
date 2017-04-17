namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Score_Sync_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Escape_1_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.Escape_2_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.Escape_3_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.Game1_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.Game2_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.Mastermind_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.Questions_UserScore", "timeStamp", c => c.Long(nullable: false));
            AddColumn("dbo.UserQuestions", "Correct", c => c.Int(nullable: false));
            AddColumn("dbo.Whackamole_UserScore", "timeStamp", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Whackamole_UserScore", "timeStamp");
            DropColumn("dbo.UserQuestions", "Correct");
            DropColumn("dbo.Questions_UserScore", "timeStamp");
            DropColumn("dbo.Mastermind_UserScore", "timeStamp");
            DropColumn("dbo.Game2_UserScore", "timeStamp");
            DropColumn("dbo.Game1_UserScore", "timeStamp");
            DropColumn("dbo.Escape_3_UserScore", "timeStamp");
            DropColumn("dbo.Escape_2_UserScore", "timeStamp");
            DropColumn("dbo.Escape_1_UserScore", "timeStamp");
        }
    }
}

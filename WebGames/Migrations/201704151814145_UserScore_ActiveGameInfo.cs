namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserScore_ActiveGameInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DayActiveGames", "SuccessMessage", c => c.String());
            AddColumn("dbo.DayActiveGames", "FailMesssage", c => c.String());
            AddColumn("dbo.DayActiveGames", "OutOfTimeMessage", c => c.String());
            AddColumn("dbo.Game1_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game2_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game3_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_1_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_2_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game4_3_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game5_UserScore", "Levels", c => c.Int(nullable: false));
            AddColumn("dbo.Game6_UserScore", "Levels", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game6_UserScore", "Levels");
            DropColumn("dbo.Game5_UserScore", "Levels");
            DropColumn("dbo.Game4_3_UserScore", "Levels");
            DropColumn("dbo.Game4_2_UserScore", "Levels");
            DropColumn("dbo.Game4_1_UserScore", "Levels");
            DropColumn("dbo.Game3_UserScore", "Levels");
            DropColumn("dbo.Game2_UserScore", "Levels");
            DropColumn("dbo.Game1_UserScore", "Levels");
            DropColumn("dbo.DayActiveGames", "OutOfTimeMessage");
            DropColumn("dbo.DayActiveGames", "FailMesssage");
            DropColumn("dbo.DayActiveGames", "SuccessMessage");
        }
    }
}

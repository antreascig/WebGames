namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Correct_Game_Names : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Game3_UserScore", newName: "Escape_1_UserScore");
            RenameTable(name: "dbo.Game6_User_Group", newName: "User_Group");
            RenameTable(name: "dbo.Game4_1_UserScore", newName: "Escape_2_UserScore");
            RenameTable(name: "dbo.Game4_2_UserScore", newName: "Escape_3_UserScore");
            RenameTable(name: "dbo.Game4_3_UserScore", newName: "Mastermind_UserScore");
            RenameTable(name: "dbo.Game5_UserScore", newName: "Questions_UserScore");
            RenameTable(name: "dbo.Game6_UserScore", newName: "Whackamole_UserScore");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Whackamole_UserScore", newName: "Game6_UserScore");
            RenameTable(name: "dbo.Questions_UserScore", newName: "Game5_UserScore");
            RenameTable(name: "dbo.Mastermind_UserScore", newName: "Game4_3_UserScore");
            RenameTable(name: "dbo.Escape_3_UserScore", newName: "Game4_2_UserScore");
            RenameTable(name: "dbo.Escape_2_UserScore", newName: "Game4_1_UserScore");
            RenameTable(name: "dbo.User_Group", newName: "Game6_User_Group");
            RenameTable(name: "dbo.Escape_1_UserScore", newName: "Game3_UserScore");
        }
    }
}

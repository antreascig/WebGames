namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Last_Game_Score_Names_Fixed : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Game1_UserScore", newName: "Adespotabalakia_UserScore");
            RenameTable(name: "dbo.Game2_UserScore", newName: "Juggler_UserScore");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Juggler_UserScore", newName: "Game2_UserScore");
            RenameTable(name: "dbo.Adespotabalakia_UserScore", newName: "Game1_UserScore");
        }
    }
}

namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameModel_Multiplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameModels", "Multiplier", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameModels", "Multiplier");
        }
    }
}

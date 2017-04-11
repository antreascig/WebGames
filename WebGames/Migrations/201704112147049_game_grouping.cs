namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class game_grouping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game6_User_Group",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Game6_User_Group");
        }
    }
}

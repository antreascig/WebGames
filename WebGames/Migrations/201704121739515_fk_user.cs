namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fk_user : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Game6_User_Group", "UserId");
            AddForeignKey("dbo.Game6_User_Group", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Game6_User_Group", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Game6_User_Group", new[] { "UserId" });
        }
    }
}

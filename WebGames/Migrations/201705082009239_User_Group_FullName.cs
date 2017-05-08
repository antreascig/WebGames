namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Group_FullName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User_Group", "User_FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User_Group", "User_FullName");
        }
    }
}

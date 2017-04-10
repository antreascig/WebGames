namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_Changes : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "SecondaryEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "SecondaryEmail", c => c.String());
        }
    }
}

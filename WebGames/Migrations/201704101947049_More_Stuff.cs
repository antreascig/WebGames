namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class More_Stuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Allowed_Email",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Email);
            
            CreateTable(
                "dbo.UserQuestions",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Questions = c.String(),
                        Answered = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.UserDailyActivities", "Timestamp", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserQuestions", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserQuestions", new[] { "UserId" });
            DropColumn("dbo.UserDailyActivities", "Timestamp");
            DropTable("dbo.UserQuestions");
            DropTable("dbo.Allowed_Email");
        }
    }
}

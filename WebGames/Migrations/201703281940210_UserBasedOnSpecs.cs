namespace WebGames.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBasedOnSpecs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "SecondaryEmail", c => c.String());
            AddColumn("dbo.AspNetUsers", "Shop", c => c.String());
            AddColumn("dbo.AspNetUsers", "MaritalStatus", c => c.String());
            AddColumn("dbo.AspNetUsers", "Hobby", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "PrivateEmail");
            DropColumn("dbo.AspNetUsers", "Birthday");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Birthday", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.AspNetUsers", "PrivateEmail", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.AspNetUsers", "Hobby");
            DropColumn("dbo.AspNetUsers", "MaritalStatus");
            DropColumn("dbo.AspNetUsers", "Shop");
            DropColumn("dbo.AspNetUsers", "SecondaryEmail");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}

using WebGames.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<GameModel> Games { get; set; }

        public DbSet<UserDailyActivity> UserDailyActivity { get; set; }

        public DbSet<DayActiveGame> DaysActiveGames { get; set; }

        public DbSet<Adespotabalakia_UserScore> Adespotabalakia_Scores { get; set; }
        public DbSet<Juggler_UserScore> Juggler_Scores { get; set; }
        public DbSet<Mastermind_UserScore> Mastermind_Scores { get; set; }
        public DbSet<Escape_1_UserScore> Escape_1_Scores { get; set; }
        public DbSet<Escape_2_UserScore> Escape_2_Scores { get; set; }
        public DbSet<Escape_3_UserScore> Escape_3_Scores { get; set; }
        public DbSet<Whackamole_UserScore> Whackamole_Scores { get; set; }
        public DbSet<Questions_UserScore> Questions_Scores { get; set; }

        public DbSet<Allowed_Email> Alowed_Emails { get; set; }

        public DbSet<UserQuestion> User_Questions { get; set; }

        public DbSet<User_Group> User_Groups { get; set; }

    }
}
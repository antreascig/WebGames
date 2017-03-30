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


        public DbSet<Game1_UserScore> Game1_Scores { get; set; }
        public DbSet<Game2_UserScore> Game2_Scores { get; set; }
        public DbSet<Game3_UserScore> Game3_Scores { get; set; }
        public DbSet<Game4_1_UserScore> Game4_1_Scores { get; set; }
        public DbSet<Game4_2_UserScore> Game4_2_Scores { get; set; }
        public DbSet<Game4_3_UserScore> Game4_3_Scores { get; set; }
        public DbSet<Game5_UserScore> Game5_Scores { get; set; }
        public DbSet<Game6_UserScore> Game6_Scores { get; set; }


    }
}
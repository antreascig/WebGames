using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class ActivityManager
    {
        public static void AddPlayedTimeForToday(string UserId, int timeInSeconds)
        {
            AddPlayedTime(UserId, DateTime.UtcNow.Date, timeInSeconds);
        }

        public static void AddPlayedTime(string UserId, DateTime Day, int timeInSeconds)
        {
            if (timeInSeconds < 0) return;

            using (var db = ApplicationDbContext.Create())
            {
                UserDailyActivity activity = GetorCreateActivity(UserId, Day, db, CreateIfNotExists:true);

                activity.TimePlayed += timeInSeconds;

                db.SaveChanges();
            }
        }
       
        public static void SetGameTime(string UserId, DateTime Day, int timeInSeconds)
        {
            if (timeInSeconds < 0) return;

            using (var db = ApplicationDbContext.Create())
            {
                UserDailyActivity activity = GetorCreateActivity(UserId, Day, db, CreateIfNotExists: true);

                activity.TimePlayed = timeInSeconds;

                db.SaveChanges();
            }
        }

        public static int GetGameTime(string UserId, DateTime Day)
        {
            int timeInSeconds = 0;
            using (var db = ApplicationDbContext.Create())
            {
                UserDailyActivity activity = GetorCreateActivity(UserId, Day, db, CreateIfNotExists: false);
                if (activity != null)
                    timeInSeconds = activity.TimePlayed;
            }
            return timeInSeconds;
        }

        private static UserDailyActivity GetorCreateActivity(string UserId, DateTime Day, ApplicationDbContext db, bool CreateIfNotExists = false)
        {
            var localizedDate = DateHelper.GetGreekDate(Day, onlyDate: true);

            var activity = db.UserDailyActivity.FindAsync(UserId, localizedDate).Result;
            if (activity == null && CreateIfNotExists)
            {
                activity.Date = localizedDate;
                activity = new UserDailyActivity();
                db.Entry<UserDailyActivity>(activity);
            }

            return activity;
        }
    }
}
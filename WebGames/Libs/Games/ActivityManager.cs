using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class GameTime
    {
        public int timeInSeconds { get; set; }
        public long timeStamp { get; set; }
    }

    public class ActivityManager
    {
        public static void SyncPlayedTimeForToday(string UserId, int timeInSeconds, long timeStamp)
        {
            SyncPlayedTime(UserId, DateTime.UtcNow.Date, timeInSeconds, timeStamp);
        }

        public static void SyncPlayedTime(string UserId, DateTime Day, int timeInSeconds, long timeStamp)
        {
            if (timeInSeconds < 0) return;

            using (var db = ApplicationDbContext.Create())
            {
                UserDailyActivity activity = GetorCreateActivity(UserId, Day, db, CreateIfNotExists:true);

                if (timeStamp > activity.Timestamp)
                {
                    activity.TimePlayed = timeInSeconds;
                    activity.Timestamp = timeStamp;

                    db.SaveChanges();
                }               
            }
        }
       
        public static void SetGameTime(string UserId, DateTime Day, int timeInSeconds)
        {
            if (timeInSeconds < 0) return;

            using (var db = ApplicationDbContext.Create())
            {
                UserDailyActivity activity = GetorCreateActivity(UserId, Day, db, CreateIfNotExists: true);

                activity.TimePlayed = timeInSeconds;
                activity.Timestamp = DateHelper.GetGreekDate(DateTime.UtcNow, onlyDate: true).Ticks;

                db.SaveChanges();
            }
        }

        public static GameTime GetGameTime(string UserId, DateTime Day)
        {
            var res = new GameTime()
            {
                timeInSeconds = 0,
                timeStamp = 0
            };
            using (var db = ApplicationDbContext.Create())
            {
                UserDailyActivity activity = GetorCreateActivity(UserId, Day, db, CreateIfNotExists: false);
                if (activity != null)
                {
                    res.timeInSeconds = activity.TimePlayed;
                    res.timeStamp = activity.Timestamp;
                }
            }
            return res;
        }

        private static UserDailyActivity GetorCreateActivity(string UserId, DateTime Day, ApplicationDbContext db, bool CreateIfNotExists = false)
        {
            var localizedDate = DateHelper.GetGreekDate(Day, onlyDate: true);

            var activity = db.UserDailyActivity.FindAsync(UserId, localizedDate).Result;
            if (activity == null && CreateIfNotExists)
            {
                activity.Date = localizedDate;
                activity = new UserDailyActivity();
                db.UserDailyActivity.Add(activity);
            }

            return activity;
        }
    }
}
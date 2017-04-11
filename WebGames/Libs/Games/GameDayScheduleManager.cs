using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class GameDayScheduleManager
    {
        public static void SaveSchedule(List<DayActiveGame> Schedule)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var CurrentSchedule = (from ag in db.DaysActiveGames select ag).ToDictionary(k => k.Day);
                foreach (var daySchedule in Schedule)
                {
                    // Make sure the day is in correct format
                    var parts = (daySchedule.Day ?? "").Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 3) throw new ArgumentException("Invalid Day Format");
                    int year, month, day;
                    if (!int.TryParse(parts[0], out year) || !int.TryParse(parts[1], out month) || !int.TryParse(parts[2], out day)) throw new ArgumentException("Invalid Day Format");
                    var Date = new DateTime(year, month, day);
                    // If the day is already in the db
                    if (CurrentSchedule.ContainsKey(daySchedule.Day) )
                    {
                        // change the gamekey
                        CurrentSchedule[daySchedule.Day].GameKey = daySchedule.GameKey;
                    }
                    else
                    {
                        // if not then Add it to the db
                        db.DaysActiveGames.Add(daySchedule);
                        CurrentSchedule.Add(daySchedule.Day, daySchedule);
                    }
                }
                var ScheduleDict = Schedule.ToDictionary(k => k.Day);
                // Go thorugh the existing days and remove any ones that were removed 
                foreach (var key in CurrentSchedule.Keys)
                {
                    if ( !ScheduleDict.ContainsKey(key) )
                    {
                        db.DaysActiveGames.Remove(CurrentSchedule[key]);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<DayActiveGame> GetSchedule()
        {
            var schedule = new List<DayActiveGame>();
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    schedule.AddRange(from ag in db.DaysActiveGames select ag);
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            return schedule;

        }

        public static string GetActiveGame(DateTime Today)
        {
            string ActiveGameKey = null;
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var toDayStr = Today.ToString("yyyy-MM-dd");

                    var Active = (from ag in db.DaysActiveGames where ag.Day == toDayStr select ag).FirstOrDefault();
                    if (Active != null) 
                        ActiveGameKey = Active.GameKey;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            return ActiveGameKey;
        }
    }
}
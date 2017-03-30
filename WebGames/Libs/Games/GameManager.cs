using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Helpers;
using WebGames.Libs.Games.GameTypes;

namespace WebGames.Libs
{
    public class GameManager
    {
        public static Dictionary<string, int> GameDict = new Dictionary<string, int>(); // < GameKey, GameId >
        public static Dictionary<string, string> GamePageDict = new Dictionary<string, string>(); // < GameKey, Page >

        public static void Init()
        {
            // initialize the games
        }

        public static string GetActiveGameKey()
        {
            var Today = DateHelper.GetGreekDate(DateTime.UtcNow, onlyDate: true);
            using (var db = ApplicationDbContext.Create())
            {
                var Active = (from ag in db.DaysActiveGames where ag.Day == Today select ag).FirstOrDefault();
                if (Active == null) return null;
                return Active.GameKey;
            }
        }
    }
}
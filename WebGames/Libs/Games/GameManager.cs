using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Helpers;
using WebGames.Libs.Games.GameTypes;
using WebGames.Libs.Games;
using WebGames.Models.ViewModels;

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
            return GameDayScheduleManager.GetActiveGame(Today);
        }

        public static List<GameSettings> GetGameSettings()
        {
            var settings = new List<GameSettings>();
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    settings.AddRange((from game in db.Games select game)
                                        .Select(g => new GameSettings()
                                        {
                                            GameId = g.GameId,
                                            Name = g.Name,
                                            Multiplier = g.Multiplier
                                        }));
                }
            }
            catch(Exception exc)
            {
                Logger.Log(exc);
            }
            
            return settings;
        }

        public static void SetGameSettings(GameSettings model )
        {
            if (model == null) return;
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var Game = GameHelper.GetGame(model.GameId, db);
                    if (Game == null) throw new ArgumentNullException($"Game {model.Name} - {model.GameId} does not exist");
                    // Copy the settings to the model
                    Game.Multiplier = model.Multiplier;
                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }

        }


        
    }
}
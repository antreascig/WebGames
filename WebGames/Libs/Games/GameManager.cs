using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Helpers;
using WebGames.Libs.Games;
using WebGames.Models.ViewModels;

namespace WebGames.Libs
{
    public class GameData
    {
        public int GameId { get; set; }
        public string GameKey{ get; set; }
        public string Name { get; set; }
        public string Page { get; set; }
        public ScoreManager SM { get; set; }
    }

    public class GameKeys
    {
        public const string GAME_1 = "GAME_1";
        public const string GAME_2 = "GAME_2";
        public const string GAME_3 = "GAME_3";
        public const string GAME_4_1 = "GAME_4_1";
        public const string GAME_4_2 = "GAME_4_2";
        public const string GAME_4_3 = "GAME_4_3";
        public const string GAME_5 = "GAME_5";
        public const string GAME_6 = "GAME_6";
    }

    public class GameManager
    {
        public static Dictionary<string, GameData> GameDict = new Dictionary<string, GameData>(); // < GameKey, GameId >

        static GameManager()
        {
            var Games = new List<GameData>()
            {
                new GameData() { GameKey = GameKeys.GAME_1, Name = "Game 1", Page = GameKeys.GAME_1, SM = new ScoreManager<Game1_UserScore>(GameKeys.GAME_1) }
            };
            using (var db = ApplicationDbContext.Create())
            {
                var ExistingGames = db.Games.ToList().ToDictionary(k => k.GameKey);
                foreach (var game in Games)
                {
                    if (ExistingGames.ContainsKey(game.GameKey))
                    {
                        game.GameId = ExistingGames[game.GameKey].GameId;
                        if (ExistingGames[game.GameKey].Name != game.Name)
                        {
                            ExistingGames[game.GameKey].Name = game.Name;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        var GameModel = new GameModel()
                        {
                            GameKey = game.GameKey,
                            Name = game.Name,
                        };
                        db.Games.Add(GameModel);
                        db.SaveChanges();
                        game.GameId = GameModel.GameId;
                    }
                    GameDict.Add(game.GameKey, game);
                }
            }
        }

        public static string GetActiveGameKey()
        {
            var Today = DateHelper.GetGreekDate(DateTime.UtcNow, onlyDate: true);
            var ActiveGame = GameDayScheduleManager.GetActiveGame(Today) ?? "";
            if (!GameDict.ContainsKey(ActiveGame)) return "";
            return ActiveGame;
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
                                            GameKey = g.GameKey,
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
                var GameId = GameManager.GameDict[model.GameKey].GameId;
                using (var db = ApplicationDbContext.Create())
                {
                    var Game = GameHelper.GetGame(GameId, db);
                    if (Game == null) throw new ArgumentNullException($"Game {model.Name} - {model.GameKey} does not exist");
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
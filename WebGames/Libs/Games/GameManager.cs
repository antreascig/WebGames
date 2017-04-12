using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Helpers;
using WebGames.Libs.Games;
using WebGames.Models.ViewModels;
using WebGames.Libs.Games.Games;

namespace WebGames.Libs
{
    public class GameData
    {
        public int GameId { get; set; }
        public string GameKey{ get; set; }
        public string Name { get; set; }
        public string PageFolder { get; set; }
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
                new GameData() { GameKey = GameKeys.GAME_1, Name = "Game 1", PageFolder = GameKeys.GAME_1, SM = new ScoreManager<Game1_UserScore>(GameKeys.GAME_1) },
                new GameData() { GameKey = GameKeys.GAME_2, Name = "Game 2", PageFolder = GameKeys.GAME_2, SM = new ScoreManager<Game2_UserScore>(GameKeys.GAME_2) },
                new GameData() { GameKey = GameKeys.GAME_3, Name = "Mastermind", PageFolder = "Mastermind", SM = new ScoreManager<Game3_UserScore>(GameKeys.GAME_3) },
                new GameData() { GameKey = GameKeys.GAME_4_1, Name = "Game 4-1", PageFolder = GameKeys.GAME_4_1, SM = new ScoreManager<Game4_1_UserScore>(GameKeys.GAME_4_1) },
                new GameData() { GameKey = GameKeys.GAME_4_2, Name = "Game 4-2", PageFolder = GameKeys.GAME_4_2, SM = new ScoreManager<Game4_2_UserScore>(GameKeys.GAME_4_2) },
                new GameData() { GameKey = GameKeys.GAME_4_3, Name = "Game 4-3", PageFolder = GameKeys.GAME_4_3, SM = new ScoreManager<Game4_3_UserScore>(GameKeys.GAME_4_3) },
                new GameData() { GameKey = GameKeys.GAME_5, Name = "Game 5", PageFolder = GameKeys.GAME_5, SM = new ScoreManager<Game5_UserScore>(GameKeys.GAME_5) },
                new GameData() { GameKey = GameKeys.GAME_6, Name = "Game 6", PageFolder = GameKeys.GAME_6, SM = new ScoreManager<Game6_UserScore>(GameKeys.GAME_6) },
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

        public static string GetActiveGameKey(string UserId)
        {
            var Today = DateHelper.GetGreekDate(DateTime.UtcNow, onlyDate: true);
            var ActiveGame = GameDayScheduleManager.GetActiveGame(Today) ?? "";
            if (!GameDict.ContainsKey(ActiveGame)) return "";

            // Check for remaining time

            var remTime = UserGameManager.GetUserGameInfo(UserId);
            if (remTime == null || remTime.RemainingTimeInSeconds <= 0)
            {
                ActiveGame = "Outoftime";
            }

            // If game6 check if user is allowed to play it
            if (ActiveGame == GameKeys.GAME_6)
            {
                var Group = Game6_Manager.GetUserGroup(UserId);
                if (Group == -1)
                {
                    ActiveGame = "";
                }
            }

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
                                            GameId = g.GameId,
                                            GameKey = g.GameKey,
                                            Name = g.Name,
                                            Multiplier = g.Multiplier
                                        }).ToList().Where(g => GameManager.GameDict.ContainsKey(g.GameKey)));
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
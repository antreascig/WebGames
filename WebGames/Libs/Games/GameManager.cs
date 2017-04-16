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
        public bool LevelAsPage = false;
        public int AvailableLevels { get; set; }
        public ScoreManager SM { get; set; }
    }

    public class GameKeys
    {
        public const string GAME_1 = "GAME_1";
        public const string GAME_2 = "GAME_2";
        public const string Mastermind = "mastermind";
        public const string GAME_4_1 = "GAME_4_1";
        public const string GAME_4_2 = "GAME_4_2";
        public const string GAME_4_3 = "GAME_4_3";
        public const string GAME_5 = "GAME_5";
        public const string GAME_6 = "GAME_6";
    }

    public class ActiveUserGameInfo
    {
        public ActiveGameData ActiveGameDataModel { get; set; }

        public string PageFolder { get; set; }

        public int RemainingTime { get; set; }

        public double GameScore { get; set; }

        public bool LevelAsPage { get; set; }

        public int ActiveLevel{ get; set; }

        public int AvailableLevels { get; set; }
    }

    public class ActiveGameData
    {
        public string ActiveGameKey { get; set; }

        public Dictionary<string, string> Messages { get; set; }
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
                new GameData() { GameKey = GameKeys.Mastermind, Name = "Mastermind", PageFolder = "mastermind", SM = new ScoreManager<Game3_UserScore>(GameKeys.Mastermind) },
                new GameData() { GameKey = GameKeys.GAME_4_1, Name = "Game 4-1", PageFolder = GameKeys.GAME_4_1, SM = new ScoreManager<Game4_1_UserScore>(GameKeys.GAME_4_1), LevelAsPage = true },
                new GameData() { GameKey = GameKeys.GAME_4_2, Name = "Game 4-2", PageFolder = GameKeys.GAME_4_2, SM = new ScoreManager<Game4_2_UserScore>(GameKeys.GAME_4_2), LevelAsPage = true },
                new GameData() { GameKey = GameKeys.GAME_4_3, Name = "Game 4-3", PageFolder = GameKeys.GAME_4_3, SM = new ScoreManager<Game4_3_UserScore>(GameKeys.GAME_4_3), LevelAsPage = true },
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

        public static ActiveUserGameInfo GetActiveGameInfo(string UserId)
        {
            var data = new ActiveUserGameInfo()
            {
                //ActiveGameData = new ActiveGameData() { ActiveGameKey = "", Messages = new Dictionary<string, string>() },
                RemainingTime = 0,
                GameScore = 0,
                LevelAsPage = false,
                ActiveLevel = 1,
                AvailableLevels = 0
            };

            // Check for active game data
            data.ActiveGameDataModel = GameDayScheduleManager.GetActiveGame(DateTime.UtcNow);
            if (data.ActiveGameDataModel != null && !GameDict.ContainsKey(data.ActiveGameDataModel.ActiveGameKey))
            {
                data.ActiveGameDataModel = null;
            }

            if (data.ActiveGameDataModel == null)
            {
                return data;
            }

            // Check for remaining time
            data.RemainingTime = UserGameManager.GetUserRemainingTime(UserId).RemainingTimeInSeconds;

            var ActiveGameKey = data.ActiveGameDataModel.ActiveGameKey;

            var gameData = GameManager.GameDict[ActiveGameKey];
            // Get Page/Folder
            data.PageFolder = gameData.PageFolder;

            // Get score for the game
            data.GameScore = gameData.SM.GetUserScore(UserId).Score;
            data.ActiveLevel = gameData.SM.GetUserScore(UserId).Levels + 1;
            data.AvailableLevels = gameData.AvailableLevels;
            // Check if level is showed as different page
            data.LevelAsPage = GameDict[ActiveGameKey].LevelAsPage;

            // If game6 check if user is allowed to play it
            if (ActiveGameKey == GameKeys.GAME_6)
            {
                var Group = Game6_Manager.GetUserGroup(UserId);
                if (Group == -1)
                {
                    data.ActiveGameDataModel.ActiveGameKey = "";
                }
            }

            return data;
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
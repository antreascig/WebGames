using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.JQuery.DataTables;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class UserAllScores
    {
        public Dictionary<string, double> GameScoreDict { get; set; }
    }

    public class UserScore
    {
        public int GameId { get; set; }
        public string GameName { get; set; }        
        public string UserId { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }
    }

    public class UserScore_Admin : UserScore
    {
        public int Tokens { get; set; }
    }

    public abstract class ScoreManager
    {
        public static void SetGenericScoreTokens(string GameKey, string UserId, int Tokens, bool Override = false)
        {
            if (!GameManager.GameDict.ContainsKey(GameKey)) return;
            var Game = GameManager.GameDict[GameKey];

            Game.SM.SetUserScore(UserId, Tokens, Override);
        }

        public static Dictionary<string, double> GetUserTotalScores(string UserId)
        {
            var res = new Dictionary<string, double>();
            using (var db = ApplicationDbContext.Create())
            {
                var User = (from u in db.Users where u.Id == UserId select u).SingleOrDefault();
                if (User != null)
                {
                    foreach ( var game in GameManager.GameDict)
                    {
                        res.Add(game.Key, 0);
                        var Game1Score = game.Value.SM.GetUserScore(User);
                        if (Game1Score != null) res[game.Key] = Game1Score.Score;
                    }
                }               
            }
            return res;
        }

        public abstract void SetUserScore(string UserId, int Tokens, bool EnableOverride = false);

        public abstract DataTablesResult GetUserScores(DataTablesParam dataTableParam);

        public abstract UserScore GetUserScore(ApplicationUser User);

        // HELPERS
        public static UserScore_Admin GenerateUserScore(GameModel Game, ApplicationUser User, int Tokens)
        {
            var res = new UserScore_Admin()
            {
                GameId = Game.GameId,
                GameName = Game.Name,
                UserId = User.Id,
                Name = User.FullName,
                Tokens = Tokens,
                Score = Game.Multiplier * Tokens
            };
            return res;
        }
    }

    public class ScoreManager<T>: ScoreManager where T: A_UserScore, new()
    {
        string GameKey { get; set; }
        public ScoreManager(string GK)
        {
            GameKey = GK;
        }

        public override UserScore GetUserScore(ApplicationUser User)
        {
            try
            {
                var GameData = GameManager.GameDict[GameKey];
                using (var db = ApplicationDbContext.Create())
                {
                    var Game = (from game in db.Games where game.GameId == GameData.GameId select game).SingleOrDefault();

                    var data = (from score in db.Set<T>() where score.UserId == User.Id select score).SingleOrDefault();

                    var Tokens = 0;
                    if (data != null)
                    {
                        Tokens = data.Tokens;
                    }

                    var res = GenerateUserScore(Game, User, Tokens);
                    return res;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        public override DataTablesResult GetUserScores(DataTablesParam dataTableParam)
        {
            try
            {
                var GameData = GameManager.GameDict[GameKey];
                using ( var db = ApplicationDbContext.Create() )
                {
                    var Game = (from game in db.Games where game.GameId == GameData.GameId select game).SingleOrDefault();

                    var data = db.Users.Join(db.Set<T>(), u => u.Id, s => s.UserId, (i,o) => new
                    {
                        UserId = i.Id,
                        Name = i.FullName,
                        Tokens = o.Tokens
                    }).ToList();
                    var ScoreData = db.Set<T>().Include("User").AsQueryable();
                    var res = DataTablesResult.Create<T>(ScoreData, dataTableParam, row => GenerateUserScore(Game, row.User, row.Tokens ));
                    return res;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        public override void SetUserScore(string UserId, int Tokens, bool EnableOverride = false)
        {
            try
            {
                using (var db = ApplicationDbContext.Create())
                {

                    var Entity = (from score in db.Set<T>() where score.UserId == UserId select score).SingleOrDefault();
                    if (Entity == null)
                    {
                        Entity = new T()
                        {
                            UserId = UserId,
                            Tokens = Tokens,
                        };
                        db.Set<T>().Add(Entity);
                    }
                    else if (EnableOverride)
                    {
                        Entity.Tokens = Tokens;
                    }
                    else if (GameKey == "Game2")
                    {
                        Entity.Tokens += Tokens;
                    }
                    else
                    {
                        return;
                    }

                    db.SaveChanges();
                }
            }
            catch(Exception exc)
            {
                Logger.Log(exc);
            }
        }
    }
}